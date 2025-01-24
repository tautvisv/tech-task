using Claims.Domain.Exceptions;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Domain.Models
{
    public class Claim
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("coverId")]
        public string CoverId { get; set; }

        [BsonElement("created")]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Created { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("claimType")]
        public ClaimType Type { get; set; }

        [BsonElement("damageCost")]
        public decimal DamageCost { get; set; }

        // it is used by entity framework. Ideally it should not exist and entity framework should have its own models
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Claim()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {

        }

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

            if (damageCost < 0)
            {
                throw new DomainValidationException("Damage cost must be zero or positive number", nameof(damageCost));
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
            return new Claim(id, coverId, created, name, type,damageCost);
        }
    }
}
