using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Xunit;

namespace Claims.Tests.Domain
{
    public class ClaimTests
    {
        [Fact]
        public void Create_WhenValidParametersArePassed_ThenClaimCreated()
        {
            // Arrange
            string coverId = "cover-123";
            DateTime created = DateTime.UtcNow;
            string name = "Test Claim";
            ClaimType type = ClaimType.Fire;
            decimal damageCost = 50m;

            // Act
            var claim = Claim.Create(coverId, created, name, type, damageCost);

            // Assert
            Assert.NotNull(claim);
            Assert.Equal(coverId, claim.CoverId);
            Assert.Equal(created, claim.Created);
            Assert.Equal(name, claim.Name);
            Assert.Equal(type, claim.Type);
            Assert.Equal(damageCost, claim.DamageCost);
            Assert.False(string.IsNullOrEmpty(claim.Id));
        }

        [Fact]
        public void Create_WhenCoverIdIsEmpty_ThenThrowsException()
        {
            // Arrange
            string coverId = "";
            DateTime created = DateTime.UtcNow;
            string name = "Test Claim";
            ClaimType type = ClaimType.Fire;
            decimal damageCost = 50m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidationException>(() => Claim.Create(coverId, created, name, type, damageCost));
            Assert.Equal("CoverId cannot be empty", ex.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100.1)]
        [InlineData(109)]
        public void Create_WhenDamageCostIsOutOfRange_ThenThrowsDomainValidationException(decimal invalidDamageCost)
        {
            // Arrange
            string coverId = "cover-123";
            DateTime created = DateTime.UtcNow;
            string name = "Test Claim";
            ClaimType type = ClaimType.Fire;

            Assert.Throws<DomainValidationException>(() => Claim.Create(coverId, created, name, type, invalidDamageCost));
        }

        [Fact]
        public void Create_WhenNameIsNull_ThenThrowsDomainValidationException()
        {
            // Arrange
            string coverId = "cover-123";
            DateTime created = DateTime.UtcNow;
            string name = null!;
            ClaimType type = ClaimType.Fire;
            decimal damageCost = 50m;

            // Act
            var claim = Claim.Create(coverId, created, name, type, damageCost);

            // Assert
            Assert.NotNull(claim);
            Assert.Null(claim.Name);
        }
    }
}
