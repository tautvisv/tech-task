using Claims.Domain.Exceptions;

namespace Claims.Domain.Models;

public class Cover
{
    public string Id { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public CoverType Type { get; private set; }
    public decimal Premium { get; private set; }

    private Cover(string id, DateOnly startDate, DateOnly endDate, CoverType type, decimal premium)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new DomainValidationException("Id cannot be empty", nameof(id));
        }

        if(premium < 0)
        {
            throw new DomainValidationException("Premium must be zero or positive number", nameof(premium));
        }

        if(startDate.CompareTo(endDate) > 0)
        {
            throw new DomainValidationException("startDate must be before endDate", nameof(startDate));
        }

        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Premium = premium;
    }

    public bool IsClaimDateValid(DateTime datetime)
    {
        var date = DateOnly.FromDateTime(datetime);
        return StartDate <= date && EndDate >= date;
    }

    public static Cover Create(DateOnly startDate, DateOnly endDate, CoverType type, decimal premium)
    {
        string id = Guid.NewGuid().ToString();
        return new Cover(id, startDate, endDate, type, premium);
    }
}
