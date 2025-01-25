using AutoMapper;
using Claims.Domain.Models;

namespace Claims.Infrastructure.MappingProfiles
{
    public class CoverMappingProfile : Profile
    {
        public CoverMappingProfile()
        {
            CreateMap<Cover, CoverEntity>();
            CreateMap<CoverEntity, Cover>();
        }
    }
}
