using Azure.Core;
using Claims.Application.Models;
using Claims.Domain.Models;

namespace Claims.Application.Services
{
    public interface IClaimService
    {
        public Task<Claim> GetClaimAsync(string id);
        public Task<IEnumerable<Claim>> GetClaimsAsync();
        public Task<Claim> CreateClaimAsync(NewClaim newClaim);
        public Task DeleteClaimAsync(string id);
    }
}
