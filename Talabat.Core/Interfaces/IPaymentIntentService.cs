using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Interfaces
{
    public interface IPaymentIntentService
    {

        public Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        public Task<Order> UpdatePaymentIntentToSucceedOrFaild(string IntentId, bool IsSucceed);
        
    }
}
