using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class OrderItems:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl {  get; set; }
        public decimal Price {  get; set; }
        public int Quantity {  get; set; }


    }
}
