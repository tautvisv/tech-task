using Claims.Domain.Models;

namespace Claims.Controllers.Models
{
    public class NewCoverDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CoverType Type { get; set; }
    }
}
