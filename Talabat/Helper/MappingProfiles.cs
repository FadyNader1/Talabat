using AutoMapper;
using Talabat.Core.Entities.Order;
using Talabat.Core.Entities.Products;
using Talabat.DTO;
using Talabat.DTO.OrderDTO;

namespace Talabat.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(x => x.ProductBrand_Name, o => o.MapFrom(x => x.ProductBrand.Name))
                .ForMember(x => x.ProductType_Name, o => o.MapFrom(x => x.ProductType.Name))
                .ForMember(x => x.PictureUrl, o => o.MapFrom<ResolvePictureUrl>())
                .ReverseMap();

            CreateMap<UserAddress, Address>();
        }
    }
}
