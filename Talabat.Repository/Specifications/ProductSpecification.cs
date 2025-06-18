using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Products;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public class ProductSpecification:BaseSpecification<Product>
    {

        public ProductSpecification(ProductParams productParams)
        {
            INclude.Add(x => x.ProductBrand);
            INclude.Add(x => x.ProductType);

            if (productParams.Brand_Id.HasValue)
                WHere = x => x.ProductBrand_Id == productParams.Brand_Id.Value;

            if(productParams.Type_Id.HasValue)
                WHere = x => x.ProductType_Id == productParams.Type_Id.Value;

            if(!string.IsNullOrEmpty(productParams.SearchByName))
                WHere = x => x.Name.ToLower().Contains(productParams.SearchByName.ToLower());



            ApplyPaggination(productParams.PageSze * (productParams.PageIndex - 1), productParams.PageSze);

        }
        public ProductSpecification(int id):base(x=>x.Id==id)
        {
            INclude.Add(x => x.ProductBrand);
            INclude.Add(x => x.ProductType);
        }
    }
}
