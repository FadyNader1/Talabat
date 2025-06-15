using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class Order:BaseEntity
    {
        public string EmailBuyer {  get; set; }
        public DateTimeOffset Date {  get; set; }= DateTimeOffset.Now;
        public Status status { get; set; } = Status.Pending;
        public ICollection<OrderItems> Items { get; set; }=new HashSet<OrderItems>();
        
        public Address ShippinAddress { get; set; }
        [ForeignKey("Delivery_Method")]
        public int? DeliveryMethodId {  get; set; }
        public DeliveryMethod? Delivery_Method { get; set; }
        public decimal SubTotal {  get; set; }
        public decimal Total()
            => SubTotal + (Delivery_Method?.Cost ?? 0);

        public string? PaymentIntentId {  get; set; }

    }
}
