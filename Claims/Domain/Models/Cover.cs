using Claims.Domain.Exceptions;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Domain.Models;

public class Cover
{
    [BsonId]
    public string Id { get; private set; }

    [BsonElement("startDate")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateOnly StartDate { get; private set; }

    [BsonElement("endDate")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateOnly EndDate { get; private set; }

    [BsonElement("claimType")]
    public CoverType Type { get; private set; }

    [BsonElement("premium")]
    public decimal Premium { get; private set; }

    // it is used by entity framework. Ideally it should not exist and entity framework should have its own models
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Cover()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        
    }

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

    public bool IsClaimDateValid(DateOnly date)
    {
        return StartDate <= date && EndDate >= date;
    }

    public static Cover Create(DateOnly startDate, DateOnly endDate, CoverType type, decimal premium)
    {
        string id = Guid.NewGuid().ToString();
        return new Cover(id, startDate, endDate, type, premium);
    }
}
