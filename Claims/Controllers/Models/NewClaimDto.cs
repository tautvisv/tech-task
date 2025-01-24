using Claims.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Claims.Controllers.Models
{
    public class NewClaimDto
    {
        [Required]
        public string? CoverId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public ClaimType? Type { get; set; }

        [Required]
        public decimal? DamageCost { get; set; }
    }
}
