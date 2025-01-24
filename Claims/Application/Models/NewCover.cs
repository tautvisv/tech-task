using Claims.Domain.Models;

namespace Claims.Application.Models;

public class NewCover
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CoverType Type { get; set; }
}
