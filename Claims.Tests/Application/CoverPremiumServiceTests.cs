using Claims.Application.Services;
using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Claims.Tests.Domain
{
    public class CoverPremiumServiceTests
    {
        private readonly Mock<ILogger<IPremiumService>> _mockLogger;
        private readonly CoverPremiumService _coverPremiumService;

        public CoverPremiumServiceTests()
        {
            _mockLogger = new Mock<ILogger<IPremiumService>>();
            _coverPremiumService = new CoverPremiumService(_mockLogger.Object);
        }

        [Fact]
        public async Task ComputePremiumAsync_WhenEndDateIsBeforeStartDate_ThenThrowsDomainValidationException()
        {
            var startDate = new DateOnly(2024, 1, 25);
            var endDate = new DateOnly(2024, 1, 1);

            await Assert.ThrowsAsync<DomainValidationException>(async () =>
                await _coverPremiumService.ComputePremiumAsync(startDate, endDate, CoverType.Yacht));
        }

        [Theory]
        [InlineData(CoverType.Yacht, 10, 13750)]
        [InlineData(CoverType.Tanker, 10, 18750)]
        [InlineData(CoverType.BulkCarrier, 10, 16250)]
        [InlineData(CoverType.PassengerShip, 10, 15000)]
        [InlineData(CoverType.ContainerShip, 10, 16250)]
        [InlineData(CoverType.Yacht, 130, 171875)]
        [InlineData(CoverType.Tanker, 130, 240000)]
        [InlineData(CoverType.BulkCarrier, 130, 208000)]
        [InlineData(CoverType.PassengerShip, 130, 192000)]
        [InlineData(CoverType.ContainerShip, 130, 208000)]
        [InlineData(CoverType.Yacht, 280, 363893.75)]
        [InlineData(CoverType.Tanker, 280, 513787.5)]
        [InlineData(CoverType.BulkCarrier, 280, 445282.5)]
        [InlineData(CoverType.PassengerShip, 280, 411030)]
        [InlineData(CoverType.ContainerShip, 280, 445282.5)]
        public async Task ComputePremiumAsync_WhenValidInputs_ThenReturnsExpectedPremium(CoverType coverType, int totalDays, decimal expectedPremium)
        {
            var startDate = new DateOnly(2024, 1, 1);
            var endDate = startDate.AddDays(totalDays);

            var premium = await _coverPremiumService.ComputePremiumAsync(startDate, endDate, coverType);

            Assert.Equal(expectedPremium, premium);
        }

        [Fact]
        public async Task ComputePremiumAsync_WhenDurationIsZero_ThenReturnsZero()
        {
            var date = new DateOnly(2024, 1, 1);

            var premium = await _coverPremiumService.ComputePremiumAsync(date, date, CoverType.Yacht);

            Assert.Equal(0, premium);
        }
    }
}
