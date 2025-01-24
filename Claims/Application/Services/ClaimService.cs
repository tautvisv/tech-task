using Claims.Application.Models;
using Claims.Application.Repositories;
using Claims.Domain.Models;
using Claims.Infrastructure.Persistance;
using Claims.Utils;

namespace Claims.Application.Services
{
    public class ClaimService : IClaimService
    {
        private readonly ILogger _logger;
        private readonly IAuditClaimPublisher _auditPublisher;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRepository<Claim> _repository;

        public ClaimService(ILogger<ClaimService> logger, ClaimsContext claimsContext, IAuditClaimPublisher auditPublisher, IDateTimeService dateTimeService, IRepository<Claim> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _auditPublisher = auditPublisher ?? throw new ArgumentNullException(nameof(auditPublisher));
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            _logger.LogInformation("Getting all claims");
            IEnumerable<Claim> claims = await _repository.GetAllAsync();
            return claims;
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            _logger.LogInformation("Getting {id} claim", id);
            var claim = await _repository.GetByIdAsync(id);
            return claim;
        }

        public async Task<Claim> CreateClaimAsync(NewClaim newClaim)
        {
            _logger.LogInformation("Creating new claim for {coverId} cover", newClaim.CoverId);
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
            await _repository.CreateAsync(claim);
            await _auditPublisher.PublishClaimCreatedAsync(id);
            return claim;
        }

        public async Task DeleteClaimAsync(string id)
        {
            _logger.LogInformation("Deleting {id} claim", id);
            await _repository.DeleteAsync(id);
            await _auditPublisher.PublishClaimDeletedAsync(id);
        }
    }
}
