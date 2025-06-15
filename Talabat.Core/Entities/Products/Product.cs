using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Products
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("ProductBrand")]
        public int ProductBrand_Id { get; set; }
        [ForeignKey("ProductType")]
        public int ProductType_Id { get; set; }
        public ProductBrand ProductBrand { get; set; }

        public ProductType ProductType { get; set; }

    }
}
