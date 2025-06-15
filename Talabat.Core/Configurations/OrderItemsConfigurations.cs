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
    public class OrderItemsConfigurations : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");


        }
    }
}
