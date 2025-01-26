using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Xunit;

namespace Claims.Tests.Domain
{
    public class CoverTests
    {
        [Theory]
        [InlineData(-0.01)]
        [InlineData(-10)]
        [InlineData(-99999)]
        public void Create_WhenPremiumIsNegative_ThenThrowsDomainValidationException(decimal invalidPremium)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(() =>
                Cover.Create(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)), CoverType.Yacht, invalidPremium)
            );
        }

        public static IEnumerable<object[]> InvalidDateRanges()
        {
            yield return new object[] { new DateOnly(2024, 1, 26), new DateOnly(2024, 1, 25) };
            yield return new object[] { new DateOnly(2024, 11, 25), new DateOnly(2024, 1, 25) };
            yield return new object[] { new DateOnly(2024, 1, 25), new DateOnly(2022, 1, 2) };
        }

        [Theory]
        [MemberData(nameof(InvalidDateRanges))]
        public void Create_WhenStartDateIsAfterEndDate_ThenThrowsException(DateOnly startDate, DateOnly endDate)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(() =>
                Cover.Create(startDate, endDate, CoverType.ContainerShip, 100)
            );

            Assert.Equal("startDate must be before endDate", exception.Message);
        }

        [Fact]
        public void Create_WhenParametersAreValid_ThenReturnCover()
        {
            // Arrange
            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10));
            var premium = 150m;

            // Act
            var cover = Cover.Create(startDate, endDate, CoverType.ContainerShip, premium);

            // Assert
            Assert.NotNull(cover);
            Assert.NotEmpty(cover.Id);
            Assert.Equal(startDate, cover.StartDate);
            Assert.Equal(endDate, cover.EndDate);
            Assert.Equal(premium, cover.Premium);
            Assert.Equal(CoverType.ContainerShip, cover.Type);
        }

        public static IEnumerable<object[]> ValidClaimDatesInRange()
        {
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), new DateTime(2024, 1, 1) };
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), new DateTime(2024, 1, 25) };
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), new DateTime(2024, 1, 13) };
        }

        [Theory]
        [MemberData(nameof(ValidClaimDatesInRange))]
        public void IsClaimDateValid_WhenDateWithinRange_ThenReturnTrue(DateOnly startDate, DateOnly endDate, DateTime claimDate)
        {
            // Arrange
            var cover = Cover.Create(startDate, endDate, CoverType.Yacht, 100);

            // Act
            bool result = cover.IsClaimDateValid(claimDate);

            // Assert
            Assert.True(result);
        }

        public static IEnumerable<object[]> InvalidClaimDatesOutOfRange()
        {
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), new DateTime(2023, 12, 30) };
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 25), new DateTime(2024, 2, 1) };
        }

        [Theory]
        [MemberData(nameof(InvalidClaimDatesOutOfRange))]
        public void IsClaimDateValid_WhenDateIsOutOfRange_ThenReturnFalse(DateOnly startDate, DateOnly endDate, DateTime claimDate)
        {
            // Arrange
            var cover = Cover.Create(startDate, endDate, CoverType.Yacht, 100);

            // Act
            bool result = cover.IsClaimDateValid(claimDate);

            // Assert
            Assert.False(result);
        }
    }
}
