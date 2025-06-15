using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;

namespace Talabat.Core.Interfaces
{
    public interface IBasketRepository
    {

        Task<CustomerBasket> CreateOrUpdateAsyc(CustomerBasket item);
        Task<IReadOnlyList<CustomerBasket>> GetAllBasketAsync();
        Task<CustomerBasket> GetBasketByIdAsync(string id);
        Task<bool> DeleteBasketAsync(string id);
        Task<bool> ClearBasketAsync();

    }
}
