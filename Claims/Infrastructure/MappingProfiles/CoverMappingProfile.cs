using AutoMapper;
using Claims.Application.Models;
using Claims.Controllers.Models;
using Claims.Domain.Models;

namespace Claims.Infrastructure.MappingProfiles
{
    public class CoverMappingProfile : Profile
    {
        public CoverMappingProfile()
        {
            CreateMap<Cover, CoverDto>();
            CreateMap<CoverDto, Cover>();
            CreateMap<NewCoverDto, NewCover>();
        }
    }
}
