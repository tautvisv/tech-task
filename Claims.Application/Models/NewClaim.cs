using Claims.Domain.Models;

namespace Claims.Application.Models
{
    public class NewClaim
    {
        public string CoverId { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedDate { get; set; }
        public ClaimType Type { get; private set; }
        public decimal DamageCost { get; private set; }

        private NewClaim() { }

        public NewClaim(string coverId, string name, DateTime createdDate, ClaimType type, decimal damageCost)
        {
            CoverId = coverId;
            Name = name;
            CreatedDate = createdDate;
            Type = type;
            DamageCost = damageCost;
        }
    }
}
