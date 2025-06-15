using Talabat.Core.Entities.Basket;

namespace Talabat.DTO
{
    public class CustomerBasketDTO
    {
        public string Id { get; set; }
        public List<ItemBasket> ItemBasket { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
