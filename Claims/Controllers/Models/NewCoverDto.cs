using Claims.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Claims.Controllers.Models
{
    public class NewCoverDto
    {
        [Required]
        public DateOnly? StartDate { get; set; }

        [Required]
        public DateOnly? EndDate { get; set; }

        [Required]
        public CoverType? Type { get; set; }
    }
}
