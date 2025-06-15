using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Basket
{
    public class ItemBasket
    {
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
