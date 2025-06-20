﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Products;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public class ProductCountSpecification:BaseSpecification<Product>
    {
        public ProductCountSpecification(ProductParams productParams)
        {
            if (productParams.Brand_Id.HasValue)
                WHere = x => x.ProductBrand_Id == productParams.Brand_Id.Value;

            if (productParams.Type_Id.HasValue)
                WHere = x => x.ProductType_Id == productParams.Type_Id.Value;

            if (!string.IsNullOrEmpty(productParams.SearchByName))
                WHere = x => x.Name.ToLower().Contains(productParams.SearchByName.ToLower());
        }
    }
}
