using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BasketId, string EmailBuyer, int Deliverymethod_Id, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetCurrentUserOrders(string BuyerEmail);
        Task<Order> GetOrderByIdAsync(int id);
        Task<string> CancelOrderByIdAsync(int id);
        Task<IReadOnlyList<Order>> GetAllOrders();
    }
}
