namespace Talabat.DTO.OrderDTO
{
    public class OrderDTO
    {
        public string BasketId {  get; set; }
        public UserAddress ShippingAddress { get; set; }
        public int DeliveryMethodId {  get; set; }

    }
}
