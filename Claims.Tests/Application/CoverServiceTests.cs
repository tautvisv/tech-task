using Claims.Application.Models;
using Claims.Application.Repositories;
using Claims.Application.Services;
using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Claims.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Claims.Tests.Domain
{
    public class CoverServiceTests
    {
        private readonly Mock<ILogger<CoverService>> _mockLogger;
        private readonly Mock<IPremiumService> _mockPremiumService;
        private readonly Mock<IRepository<Cover>> _mockCoverRepository;
        private readonly Mock<IAuditCoverPublisher> _mockAuditCoverPublisher;
        private readonly Mock<IDateTimeService> _mockDateTimeService;
        private readonly CoverService _coverService;

        public CoverServiceTests()
        {
            _mockLogger = new Mock<ILogger<CoverService>>();
            _mockPremiumService = new Mock<IPremiumService>();
            _mockCoverRepository = new Mock<IRepository<Cover>>();
            _mockAuditCoverPublisher = new Mock<IAuditCoverPublisher>();
            _mockDateTimeService = new Mock<IDateTimeService>();

            _coverService = new CoverService(
                _mockLogger.Object,
                _mockPremiumService.Object,
                _mockCoverRepository.Object,
                _mockAuditCoverPublisher.Object,
                _mockDateTimeService.Object
            );
        }

        [Fact]
        public async Task GetCoverAsync_WhenCoverExists_ThenReturnsCover()
        {
            var cover = Cover.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 4, 1), CoverType.Yacht, 100);
            _mockCoverRepository.Setup(repo => repo.GetByIdAsync("1")).ReturnsAsync(cover);

            var result = await _coverService.GetCoverAsync("1");

            Assert.Equal(cover, result);
        }

        [Fact]
        public async Task GetCoversAsync_WhenCoversExist_ThenReturnsAllCovers()
        {
            var covers = new List<Cover> { Cover.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 4, 1), CoverType.Yacht, 100) };
            _mockCoverRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(covers);

            var result = await _coverService.GetCoversAsync();

            Assert.Equal(covers, result);
        }

        [Fact]
        public async Task CreateCoverAsync_WhenNewCoverIsValid_ThenCreatesCover()
        {
            var newCover = new NewCover { StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 4, 1), Type = CoverType.Yacht };
            _mockDateTimeService.Setup(svc => svc.GetCurrentDateTime()).Returns(new DateTime(2024, 1, 1));
            _mockPremiumService.Setup(svc => svc.ComputePremiumAsync(newCover.StartDate, newCover.EndDate, newCover.Type)).ReturnsAsync(500);

            var result = await _coverService.CreateCoverAsync(newCover);

            Assert.Equal(500, result.Premium);
            _mockCoverRepository.Verify(repo => repo.CreateAsync(It.IsAny<Cover>()), Times.Once);
            _mockAuditCoverPublisher.Verify(pub => pub.PublishCoverCreatedAsync(result.Id), Times.Once);
        }

        [Fact]
        public async Task CreateCoverAsync_WhenNewCoverStartDateIsInThePast_ThenThrowsException()
        {

            var newCover = new NewCover { StartDate = new DateOnly(2023, 1, 1), EndDate = new DateOnly(2023, 4, 1), Type = CoverType.Yacht };
            _mockDateTimeService.Setup(svc => svc.GetCurrentDateTime()).Returns(new DateTime(2024, 1, 1));

            await Assert.ThrowsAsync<DomainValidationException>(() => _coverService.CreateCoverAsync(newCover));
        }

        [Fact]
        public async Task CreateCoverAsync_WhenNewCoverExceedsOneYear_ThenThrowsException()
        {
            var newCover = new NewCover { StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2026, 4, 1), Type = CoverType.Yacht };
            _mockDateTimeService.Setup(svc => svc.GetCurrentDateTime()).Returns(new DateTime(2024, 1, 1));

            await Assert.ThrowsAsync<DomainValidationException>(() => _coverService.CreateCoverAsync(newCover));
        }

        [Fact]
        public async Task DeleteCoverAsync_WhenCoverExists_ThenDeletesCover()
        {
            await _coverService.DeleteCoverAsync("1");

            _mockCoverRepository.Verify(repo => repo.DeleteAsync("1"), Times.Once);
            _mockAuditCoverPublisher.Verify(pub => pub.PublishCoverDeletedAsync("1"), Times.Once);
        }
    }
}
