using Claims.Controllers;
using Claims.Domain.Models;
using Claims.Utils;

namespace Claims.Domain.Services
{
    public class ClaimService : IClaimService
    {
        private readonly ILogger<ClaimService> _logger;
        private readonly ClaimsContext _claimsContext;
        private readonly IAuditPublisher _auditPublisher;
        private readonly IDateTimeService _dateTimeService;

        public ClaimService(ILogger<ClaimService> logger, ClaimsContext claimsContext, IAuditPublisher auditPublisher, IDateTimeService dateTimeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _claimsContext = claimsContext ?? throw new ArgumentNullException(nameof(claimsContext));
            _auditPublisher = auditPublisher ?? throw new ArgumentNullException(nameof(auditPublisher));
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
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
            var created = _dateTimeService.GetCurrentTime();
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
            await _auditPublisher.PublishClaimCreatedAsync(id);
            return claim;
        }

        public async Task DeleteClaimAsync(string id)
        {
            await _auditPublisher.PublishClaimDeletedAsync(id);
            await _claimsContext.DeleteItemAsync(id);
        }
    }
}
