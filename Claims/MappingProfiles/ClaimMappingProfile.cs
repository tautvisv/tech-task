using AutoMapper;
using Claims.Controllers.Models;
using Claims.Domain.Models;

namespace Claims.MappingProfiles
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
