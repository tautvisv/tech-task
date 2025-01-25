using AutoMapper;
using Claims.Domain.Models;

namespace Claims.Infrastructure.MappingProfiles
{
    public class ClaimMappingProfile : Profile
    {
        public ClaimMappingProfile()
        {
            CreateMap<Claim, ClaimEntity>();
            CreateMap<ClaimEntity, Claim>();
        }
    }
}
