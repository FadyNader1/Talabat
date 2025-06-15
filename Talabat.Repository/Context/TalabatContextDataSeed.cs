using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;
using Talabat.Core.Entities.Products;

namespace Talabat.Repository.Context
{
    public class TalabatContextDataSeed
    {
        private readonly TalabatContext context;

       
        public static async Task SeedData(TalabatContext context)
        {
            if(!context.Set<ProductBrand>().Any())
            {
                var data = File.ReadAllText("../Talabat.Repository/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(data);
                if(brands != null && brands.Count()>0 )
                {
                    foreach(var x in brands)
                    {
                        await context.Set<ProductBrand>().AddAsync(x);
                        await context.SaveChangesAsync();
                    }
                }
            }
            if(!context.Set<ProductType>().Any())
            {
                var data = File.ReadAllText("../Talabat.Repository/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(data);
                if(types != null && types.Count()>0 )
                {
                    foreach(var x in types)
                    {
                        await context.Set<ProductType>().AddAsync(x);
                        await context.SaveChangesAsync();
                    }
                }
            }
            if(!context.Set<Product>().Any())
            {
                var data = File.ReadAllText("../Talabat.Repository/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(data);
                if(products != null && products.Count()>0 )
                {
                    foreach(var x in products)
                    {
                        await context.Set<Product>().AddAsync(x);
                        await context.SaveChangesAsync();
                    }
                }
            }
            if(!context.Set<DeliveryMethod>().Any())
            {
                var data = File.ReadAllText("../Talabat.Repository/DataSeed/delivery_methods.json");
                var DM= JsonSerializer.Deserialize<List<DeliveryMethod>>(data);
                if(DM != null && DM.Count()>0 )
                {
                    foreach(var x in DM)
                    {
                        await context.Set<DeliveryMethod>().AddAsync(x);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
