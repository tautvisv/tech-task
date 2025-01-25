using Claims.Domain.Exceptions;

namespace Claims.Domain.Models
{
    public class Claim
    {
        public string Id { get; set; }
        public string CoverId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public ClaimType Type { get; set; }
        // It is possible to use value objects for domain. For current functionality primitive types should be sufficient.
        public decimal DamageCost { get; set; }

        private Claim(string id, string coverId, DateTime created, string name, ClaimType type, decimal damageCost)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new DomainValidationException("Id cannot be empty", nameof(id));
            }

            if (string.IsNullOrEmpty(coverId))
            {
                throw new DomainValidationException("CoverId cannot be empty", nameof(coverId));
            }

            if (damageCost < 0 || damageCost > 100)
            {
                throw new DomainValidationException("Damage cost must be between 0 and 100", nameof(damageCost));
            }

            Id = id;
            CoverId = coverId;
            Created = created;
            Name = name;
            Type = type;
            DamageCost = damageCost;
        }

        public static Claim Create(string coverId, DateTime created, string name, ClaimType type, decimal damageCost)
        {
            var id = Guid.NewGuid().ToString();
            return new Claim(id, coverId, created, name, type, damageCost);
        }
    }
}
