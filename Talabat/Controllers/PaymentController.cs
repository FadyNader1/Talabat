using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.V2;
using System.IO;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Interfaces;

namespace Talabat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentIntentService paymentIntentService;
        private readonly IConfiguration config;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentIntentService paymentIntentService,
            IConfiguration config,
            ILogger<PaymentController> logger)
        {
            this.paymentIntentService = paymentIntentService;
            this.config = config;
            this._logger = logger;
        }

        [HttpPost("CreateOrUpdatePaymentIntent/{basketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                return BadRequest("Basket ID is required.");
            }

            var basket = await paymentIntentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null)
            {
                return NotFound("Basket not found.");
            }

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, config["Stripe:WhSecret"]);
                Console.WriteLine($"✅ Event Received: {stripeEvent.Type}");

                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                    case "payment_intent.payment_failed":
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        if (paymentIntent == null)
                        {
                            Console.WriteLine($"⚠️ Could not cast event object to PaymentIntent. Event ID: {stripeEvent.Id}");
                            return BadRequest();
                        }

                        var isSucceeded = stripeEvent.Type == "payment_intent.succeeded";
                        await paymentIntentService.UpdatePaymentIntentToSucceedOrFaild(paymentIntent.Id, isSucceeded);
                        break;

                    default:
                        Console.WriteLine($"ℹ️ Event type {stripeEvent.Type} not explicitly handled.");
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return BadRequest();
            }
        }

    }

}
