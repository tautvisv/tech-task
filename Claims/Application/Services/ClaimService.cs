using Claims.Application.Models;
using Claims.Application.Repositories;
using Claims.Domain.Exceptions;
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
        private readonly IRepository<Cover> _coverRepository;

        public ClaimService(ILogger<ClaimService> logger, ClaimsContext claimsContext, IAuditClaimPublisher auditPublisher, IDateTimeService dateTimeService, IRepository<Claim> repository, IRepository<Cover> coverRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _auditPublisher = auditPublisher ?? throw new ArgumentNullException(nameof(auditPublisher));
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _coverRepository = coverRepository ?? throw new ArgumentNullException(nameof(coverRepository));
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
            if (newClaim is null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            _logger.LogInformation("Creating new claim for {coverId} cover", newClaim.CoverId);
            var relatedCover = await _coverRepository.GetByIdAsync(newClaim.CoverId);
            var createdDateTime = _dateTimeService.GetCurrentDateTime();
            var createdDate = DateOnly.FromDateTime(createdDateTime);
            if (!relatedCover.IsClaimDateValid(createdDate))
            {
                throw new DomainValidationException($"Created date must be within the period({relatedCover.StartDate}-{relatedCover.EndDate}) of the related Cover.", nameof(createdDate));
            }

            var claim = Claim.Create(newClaim.CoverId, createdDate, newClaim.Name, newClaim.Type, newClaim.DamageCost);
            await _repository.CreateAsync(claim);
            await _auditPublisher.PublishClaimCreatedAsync(claim.Id);
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
