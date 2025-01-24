using Claims.Domain.Models;

namespace Claims.Application.Models
{
    public class NewClaim
    {
        public string CoverId { get; private set; }
        public string Name { get; private set; }
        public ClaimType Type { get; private set; }
        public decimal DamageCost { get; private set; }
    }
}
