namespace Claims.Domain.Models
{
    public class NewClaim
    {
        public string CoverId { get; set; }
        public string Name { get; set; }
        public ClaimType Type { get; set; }
        public decimal DamageCost { get; set; }
    }
}
