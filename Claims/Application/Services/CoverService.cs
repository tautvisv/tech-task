using Claims.Application.Models;
using Claims.Application.Repositories;
using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Claims.Infrastructure;
using Claims.Infrastructure.Persistance;
using Claims.Utils;

namespace Claims.Application.Services
{
    public class CoverService : ICoverService
    {
        private readonly ILogger _logger;
        private readonly IPremiumService _premiumService;
        private readonly IRepository<Cover> _coverRepository;
        private readonly IAuditCoverPublisher _auditCoverPublisher;
        private readonly IDateTimeService _dateTimeService;

        public CoverService(ILogger<CoverService> logger, IPremiumService premiumService, IRepository<Cover> coverRepository, IAuditCoverPublisher auditCoverPublisher, IDateTimeService dateTimeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _premiumService = premiumService ?? throw new ArgumentNullException(nameof(premiumService));
            _coverRepository = coverRepository ?? throw new ArgumentNullException(nameof(coverRepository));
            _auditCoverPublisher = auditCoverPublisher ?? throw new ArgumentNullException(nameof(auditCoverPublisher));
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        }

        public async Task<Cover> GetCoverAsync(string id)
        {
            _logger.LogInformation("Getting {id} cover", id);
            var cover = await _coverRepository.GetByIdAsync(id);
            return cover;
        }

        public async Task<IEnumerable<Cover>> GetCoversAsync()
        {
            _logger.LogInformation("Getting all covers");
            var covers = await _coverRepository.GetAllAsync();
            return covers;
        }

        public async Task<Cover> CreateCoverAsync(NewCover newClaim)
        {
            if (newClaim is null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            _logger.LogInformation("Creating new '{coverType}' cover ", newClaim.Type);
            var currentTime = _dateTimeService.GetCurrentDateTime();
            var currentDate = DateOnly.FromDateTime(currentTime);
            if (currentDate > newClaim.StartDate)
            {
                throw new DomainValidationException("StartDate cannot be in the past", nameof(newClaim.StartDate));
            }
            if (newClaim.StartDate.AddYears(1).CompareTo(newClaim.EndDate) > 0)
            {
                throw new DomainValidationException("Total insurance period cannot exceed 1 year", nameof(newClaim.StartDate));
            }

            var premium = await _premiumService.ComputePremiumAsync(newClaim.StartDate, newClaim.EndDate, newClaim.Type);
            var cover = Cover.Create(newClaim.StartDate, newClaim.EndDate, newClaim.Type, premium);
            await _coverRepository.CreateAsync(cover);
            await _auditCoverPublisher.PublishCoverCreatedAsync(cover.Id);
            return cover;
        }

        public async Task DeleteCoverAsync(string id)
        {
            _logger.LogInformation("Deleting {id} cover", id);
            await _coverRepository.DeleteAsync(id);
            await _auditCoverPublisher.PublishCoverDeletedAsync(id);
        }
    }
}
