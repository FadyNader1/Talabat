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
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ShortName).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.Description).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.Cost).IsRequired().HasColumnType("decimal(18,2)");


        }
    }
}
