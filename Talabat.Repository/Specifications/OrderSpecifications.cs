using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public class OrderSpecifications:BaseSpecification<Order>
    {
        public OrderSpecifications()
        {
            INclude.Add(x => x.Items);

            INclude.Add(x => x.Delivery_Method);
        }
        public OrderSpecifications(string email):base(x=>x.EmailBuyer==email)
        {
            INclude.Add(x => x.Items);

            INclude.Add(x => x.Delivery_Method);
        }
        public OrderSpecifications(int id):base(x=>x.Id==id)
        {
            INclude.Add(x => x.Items);

            INclude.Add(x => x.Delivery_Method);
        }
    }
}
