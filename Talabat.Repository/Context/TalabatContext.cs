using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;
using Talabat.Core.Entities.Products;

namespace Talabat.Repository.Context
{
    public class TalabatContext: DbContext
    {
        public TalabatContext(DbContextOptions<TalabatContext> options):base(options)
        {
            
        }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductBrand> productBrands { get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        public DbSet<DeliveryMethod> deliveryMethods { get; set; }
        public DbSet<OrderItems> orderItems { get; set; }
        public DbSet<Order> orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
