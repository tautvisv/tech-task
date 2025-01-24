using Claims.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Claims.Controllers.Models
{
    public class NewCoverDto
    {
        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [Required]
        public CoverType? Type { get; set; }
    }
}
