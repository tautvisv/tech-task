using Claims.Domain.Models;

namespace Claims.Application.Services
{
    public class CoverPremiumService : IPremiumService
    {
        private readonly ILogger _logger;

        public CoverPremiumService(ILogger<IPremiumService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<decimal> ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            _logger.LogInformation("Stating to calculate premium for {coverType}", coverType);

            var value = ComputePremium(startDate, endDate, coverType);

            _logger.LogInformation("Premium calculation finished for {coverType}", coverType);

            return Task.FromResult(value);
        }

        private decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            var multiplier = 1.3m;
            if (coverType == CoverType.Yacht)
            {
                multiplier = 1.1m;
            }

            if (coverType == CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }

            if (coverType == CoverType.Tanker)
            {
                multiplier = 1.5m;
            }

            var premiumPerDay = 1250 * multiplier;
            var insuranceLength = (endDate - startDate).TotalDays;
            var totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                if (i < 30) totalPremium += premiumPerDay;
                if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
                else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
                if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
                else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
            }

            return totalPremium;
        }
    }
}
