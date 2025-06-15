using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.ShippinAddress, y => y.WithOwner());
            builder.HasMany(x=>x.Items).WithOne();
            builder.HasOne(x => x.Delivery_Method).WithMany().OnDelete(DeleteBehavior.SetNull);
            builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");

        }
    }
}
