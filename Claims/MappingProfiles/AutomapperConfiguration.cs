using AutoMapper;
using Claims.Controllers.Models;

namespace Claims.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Map Product to ProductDTO (for customers)
            // Source: Product and Destination: ProductDTO
            CreateMap<Claim, ClaimDto>();
            // Map ProductCreateDTO to Product (for adding new product)
            // Source: ProductCreateDTO and Destination: Product
            CreateMap<ClaimDto, Claim>();
            CreateMap<Cover, CoverDto>();
            CreateMap<CoverDto, Cover>();
        }
    }
}
