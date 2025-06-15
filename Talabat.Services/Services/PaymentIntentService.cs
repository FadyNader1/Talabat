using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order;
using Talabat.Core.Interfaces;
using Talabat.Repository.Context;

namespace Talabat.Services.Services
{

    public class PaymentIntentService : IPaymentIntentService
    {

        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly TalabatContext context;
        private readonly IUnitOfWork unitOfWork;

        public PaymentIntentService()
        {
        }

        public PaymentIntentService(IConfiguration configuration, IBasketRepository basketRepository, TalabatContext context, IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.context = context;
            this.unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:secretkey"];
            var basket = await basketRepository.GetBasketByIdAsync(basketId);
            if (basket is null)
                return null;
            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await context.Set<DeliveryMethod>().FindAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod is not null)
                    shippingPrice = deliveryMethod.Cost;
            }

            PaymentIntent paymentIntent;
            var services = new Stripe.PaymentIntentService();
            foreach (var item in basket.ItemBasket)
            {
                var product = await unitOfWork.Repository<Talabat.Core.Entities.Products.Product>().GetByIdAsync(item.Product_Id);
                item.Product_Name= product.Name;
                item.Price = product.Price;
            }


            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.ItemBasket.Sum(x => x.Price * x.Quantity * 100 + shippingPrice),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await services.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.ItemBasket.Sum(x => x.Price * x.Quantity * 100 + shippingPrice),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await services.UpdateAsync(basket.PaymentIntentId, options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            await basketRepository.CreateOrUpdateAsyc(basket);

            return basket;
        }



    }
}
