using Claims.Domain.Models;

namespace Claims.Application.Models;

public class NewCover
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public CoverType Type { get; set; }
}
