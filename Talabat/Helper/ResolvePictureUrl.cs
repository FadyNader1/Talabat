using AutoMapper;
using Talabat.Core.Entities.Products;
using Talabat.DTO;

namespace Talabat.Helper
{
    public class ResolvePictureUrl : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration confg;

        public ResolvePictureUrl(IConfiguration confg)
        {
            this.confg = confg;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{confg["BaseURL"]}{source.PictureUrl}";

            return string.Empty;

        }
    }
}
