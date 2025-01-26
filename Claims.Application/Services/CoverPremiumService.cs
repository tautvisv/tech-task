using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Claims.Application.Services
{
    public class CoverPremiumService : IPremiumService
    {
        private const decimal BaseDayRate = 1250;
        private const int FirstPeriod = 30;
        private const int SecondPeriod = 150;

        private readonly ILogger _logger;

        public CoverPremiumService(ILogger<IPremiumService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<decimal> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            _logger.LogInformation("Stating to calculate premium for {coverType} from {startDate} to {endDate}", coverType, startDate, endDate);

            var value = ComputePremium(startDate, endDate, coverType);

            _logger.LogInformation("Premium calculation finished. Value: {value}", value);

            return Task.FromResult(value);
        }

        private decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            var totalDays = endDate.DayNumber - startDate.DayNumber;
            if (totalDays < 0)
            {
                throw new DomainValidationException("End date must be before start date", nameof(endDate));
            }

            var totalPremium = ComputeRate(totalDays, coverType) * BaseDayRate;
            return totalPremium;
        }

        private decimal ComputeRate(int numberOfDays, CoverType coverType)
        {
            var rates = Rates.GetRates(coverType);

            var remainingDays = numberOfDays;
            var firstLevelDays = remainingDays > FirstPeriod ? FirstPeriod : remainingDays;
            remainingDays -= FirstPeriod;
            var secondLevelDays = remainingDays > SecondPeriod ? SecondPeriod : Math.Max(0, remainingDays);
            remainingDays -= SecondPeriod;
            var thirdLevelDays = Math.Max(0, remainingDays);

            var premiumRate = firstLevelDays * rates.FirstLevel + secondLevelDays * rates.SecondLevel + thirdLevelDays * rates.ThirdLevel;
            return premiumRate;
        }

        private class Rates
        {
            private static readonly Rates Yacht = new Rates(1.1m, 0.95m, 0.97m);
            private static readonly Rates PassangerShip = new Rates(1.2m, 0.98m, 0.99m);
            private static readonly Rates Tanker = new Rates(1.5m, 0.98m, 0.99m);
            private static readonly Rates Other = new Rates(1.3m, 0.98m, 0.99m);

            public decimal FirstLevel { get; }
            public decimal SecondLevel { get; }
            public decimal ThirdLevel { get; }

            private Rates(decimal firstLevel, decimal secondLevel, decimal thirdLevel)
            {
                FirstLevel = firstLevel;
                // I have assumption that discount is applied from the first level price.
                SecondLevel = firstLevel * secondLevel;
                ThirdLevel = firstLevel * secondLevel * thirdLevel;
            }

            public static Rates GetRates(CoverType type)
            {
                switch (type)
                {
                    case CoverType.Yacht: return Yacht;
                    case CoverType.PassengerShip: return PassangerShip;
                    case CoverType.Tanker: return Tanker;
                    // add other rates if required
                    default: return Other;
                }
            }
        }
    }
}