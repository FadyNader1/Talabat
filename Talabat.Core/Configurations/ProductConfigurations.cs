using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Products;

namespace Talabat.Core.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PictureUrl).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasOne(x => x.ProductBrand).WithMany().HasForeignKey(x => x.ProductBrand_Id);
            builder.HasOne(x => x.ProductType).WithMany().HasForeignKey(x => x.ProductType_Id);
        }
    }
}
