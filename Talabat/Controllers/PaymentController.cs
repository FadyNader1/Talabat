using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Interfaces;

namespace Talabat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentIntentService paymentIntentService;

        public PaymentController(IPaymentIntentService paymentIntentService)
        {
            this.paymentIntentService = paymentIntentService;
        }
        [HttpPost("CreateOrUpdatePaymentIntent/{basketId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
                return BadRequest("Basket ID is required.");
            var basket = await paymentIntentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null)
                return NotFound("Basket not found.");
            return Ok(basket);
        }
    }
}
