using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;
using Talabat.Core.Entities.Products;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Specifications;

namespace Talabat.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<string> CancelOrderByIdAsync(int id)
        {
            var order= await GetOrderByIdAsync(id);
            if (order == null)
                return "Order not found";
            try
            {
                unitOfWork.Repository<Order>().Delete(order);
                await unitOfWork.Complete();
                return $"Cancel order successfully";

            }catch(Exception ex)
            {
                return $"Error while canceling order: {ex.Message}";
            }

        }

        public async Task<Order?> CreateOrderAsync(string BasketId, string EmailBuyer, int Deliverymethod_Id,Address shippingAddress)
        {
            var basket=await basketRepository.GetBasketByIdAsync(BasketId);
            if(basket == null) 
                return null;
            var deliverymethod=await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Deliverymethod_Id);
            if (deliverymethod == null)
                return null;
            var orderitems = new List<OrderItems>();
            if (basket.ItemBasket.Count > 0)
                foreach (var x in basket.ItemBasket)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(x.Product_Id);
                    if (product is null)
                        return null;
                    var orderitem = new OrderItems()
                    {
                        Name = product.Name,
                        Description = product.Description,
                        PictureUrl = product.PictureUrl,
                        Price = product.Price,
                        Quantity = x.Quantity,
                    };
                    orderitems.Add(orderitem);
                }
            var subtotal = orderitems.Sum(x => x.Price * x.Quantity);
            var order = new Order()
            {
                EmailBuyer = EmailBuyer,
                status = Status.Pending,
                Items = orderitems,
                Delivery_Method = deliverymethod,
                ShippinAddress = shippingAddress,
                SubTotal = subtotal,
                PaymentIntentId = basket.PaymentIntentId ?? "Not Found Payment",
            };
            await unitOfWork.Repository<Order>().AddAsync(order);
            await unitOfWork.Complete();
            return order;

        }

        public async Task<IReadOnlyList<Order>> GetAllOrders()
        {
            var spec = new OrderSpecifications();
            var orders=await unitOfWork.Repository<Order>().GetAllSpecificationAsync(spec);
            if (orders is null)
                return null;
            return orders;
        }

        public async Task<IReadOnlyList<Order>> GetCurrentUserOrders(string BuyerEmail)
        {

            var spec = new OrderSpecifications(BuyerEmail);
            var order = await unitOfWork.Repository<Order>().GetAllSpecificationAsync(spec);
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var spec=new OrderSpecifications(id);
            var order=await unitOfWork.Repository<Order>().GetByIdSpecificationAsync(spec);
            if (order == null)
                return null;
            return order;
        }
    }
}
