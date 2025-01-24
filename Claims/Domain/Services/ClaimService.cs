using Claims.Auditing;
using Claims.Controllers;
using Claims.Domain.Models;

namespace Claims.Domain.Services
{
    public class ClaimService : IClaimService
    {
        private readonly ILogger<ClaimService> _logger;
        private readonly ClaimsContext _claimsContext;
        private readonly Auditer _auditer;

        public ClaimService(ILogger<ClaimService> logger, ClaimsContext claimsContext, Auditer auditer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _claimsContext = claimsContext ?? throw new ArgumentNullException(nameof(claimsContext));
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            IEnumerable<Claim> claims = await _claimsContext.GetClaimsAsync();
            return claims;
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            var claim = await _claimsContext.GetClaimAsync(id);
            return claim;
        }

        public async Task<Claim> CreateClaimAsync(NewClaim newClaim)
        {
            var id = Guid.NewGuid().ToString();
            // TODO: fix it
            var created = DateTime.UtcNow;
            var claim = new Claim
            {
                Id = id,
                Name = newClaim.Name,
                CoverId = newClaim.CoverId,
                // it makes more sense if system is setting this date
                // in the end it depends on application requirments
                Created = created,
                Type = newClaim.Type,
                DamageCost = newClaim.DamageCost,
            };
            await _claimsContext.AddItemAsync(claim);
            _auditer.AuditClaim(claim.Id, "POST");
            return claim;
        }

        public async Task DeleteClaimAsync(string id)
        {
            _auditer.AuditClaim(id, "DELETE");
            await _claimsContext.DeleteItemAsync(id);
        }
    }
}
