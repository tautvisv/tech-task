using Claims.Application.Models;
using Claims.Application.Repositories;
using Claims.Application.Services;
using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Claims.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Claims.Tests.Application
{
    // For each project we should create a new test project. For simplicity I have added all tests into a single test project
    public class ClaimServiceTests
    {
        private readonly Mock<ILogger<ClaimService>> _mockLogger;
        private readonly Mock<IAuditClaimPublisher> _mockAuditPublisher;
        private readonly Mock<IDateTimeService> _mockDateTimeService;
        private readonly Mock<IRepository<Claim>> _mockClaimRepository;
        private readonly Mock<IRepository<Cover>> _mockCoverRepository;
        private readonly ClaimService _claimService;

        public ClaimServiceTests()
        {
            _mockLogger = new Mock<ILogger<ClaimService>>();
            _mockAuditPublisher = new Mock<IAuditClaimPublisher>();
            _mockDateTimeService = new Mock<IDateTimeService>();
            _mockClaimRepository = new Mock<IRepository<Claim>>();
            _mockCoverRepository = new Mock<IRepository<Cover>>();

            _claimService = new ClaimService(
                _mockLogger.Object,
                _mockAuditPublisher.Object,
                _mockDateTimeService.Object,
                _mockClaimRepository.Object,
                _mockCoverRepository.Object
            );
        }

        [Fact]
        public async Task GetClaimsAsync_WhenCalled_ThenReturnsAllClaims()
        {
            var claims = new List<Claim> 
            {
                Claim.Create("cover-123", new DateTime(2024, 02, 01), "Test", ClaimType.Fire, 50m)
            };
            _mockClaimRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(claims);

            var result = await _claimService.GetClaimsAsync();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetClaimAsync_WhenClaimExists_ThenReturnsClaim()
        {
            var claim = Claim.Create("cover-123", new DateTime(2024, 02, 01), "Test Claim", ClaimType.Fire, 50m);
            _mockClaimRepository
                .Setup(repo => repo.GetByIdAsync(claim.Id))
                .ReturnsAsync(claim);

            var result = await _claimService.GetClaimAsync(claim.Id);

            Assert.NotNull(result);
            Assert.Equal(claim.Id, result.Id);
        }

        [Fact]
        public async Task CreateClaimAsync_WhenNewClaimIsNull_ThenThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _claimService.CreateClaimAsync(null!));
        }

        public static IEnumerable<object[]> InvalidClaimDatesOutOfRange()
        {
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), new DateTime(2023, 12, 30) };
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), new DateTime(2024, 2, 1) };
        }

        [Theory]
        [MemberData(nameof(InvalidClaimDatesOutOfRange))]
        public async Task CreateClaimAsync_WhenClaimDateIsInvalid_ThenThrowsDomainValidationException(DateOnly coverStartDate, DateOnly coverEndDate, DateTime claimCreateDate)
        {
            var newClaim = new NewClaim("test-id", "cover-123", claimCreateDate, ClaimType.Fire, 50m);
            var cover = Cover.Create(coverStartDate, coverEndDate, CoverType.BulkCarrier, 100m);
            _mockCoverRepository.Setup(repo => repo.GetByIdAsync(newClaim.CoverId)).ReturnsAsync(cover);

            await Assert.ThrowsAsync<DomainValidationException>(() => _claimService.CreateClaimAsync(newClaim));
        }

        [Fact]
        public async Task CreateClaimAsync_WhenInvalid_ThenCreatesClai()
        {
            var newClaim = new NewClaim("test-id", "cover-123", new DateTime(2024, 1, 15), ClaimType.Fire, 50m);
            var cover = Cover.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), CoverType.BulkCarrier, 100m);
            _mockCoverRepository.Setup(repo => repo.GetByIdAsync(newClaim.CoverId)).ReturnsAsync(cover);

            var createdClaim = await _claimService.CreateClaimAsync(newClaim);
            Assert.NotNull(createdClaim);
            Assert.False(string.IsNullOrEmpty(createdClaim.Id));
        }

        [Fact]
        public async Task DeleteClaimAsync_WhenCalled_ThenDeletesClaim()
        {
            string claimId = "claim-123";

            await _claimService.DeleteClaimAsync(claimId);

            _mockClaimRepository.Verify(repo => repo.DeleteAsync(claimId), Times.Once);
            _mockAuditPublisher.Verify(pub => pub.PublishClaimDeletedAsync(claimId), Times.Once);
        }
    }
}