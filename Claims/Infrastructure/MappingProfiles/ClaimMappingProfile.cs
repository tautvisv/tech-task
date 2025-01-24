using AutoMapper;
using Claims.Application.Models;
using Claims.Controllers.Models;
using Claims.Domain.Models;

namespace Claims.Infrastructure.MappingProfiles
{
    public class ClaimMappingProfile : Profile
    {
        public ClaimMappingProfile()
        {
            CreateMap<Claim, ClaimDto>();
            CreateMap<ClaimDto, Claim>();
            CreateMap<NewClaimDto, NewClaim>();
        }
    }
}
