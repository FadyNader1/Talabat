using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Talabat.Core.Entities.Products;

namespace Talabat.DTO
{
    public class ProductDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        [Required]
        [MaxLength(1000)]
        public string PictureUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int ProductBrand_Id { get; set; }
        public string ProductBrand_Name { get; set; }

        public int ProductType_Id { get; set; }
        public string ProductType_Name { get; set; }


    }
}
