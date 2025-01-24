using Claims.Domain.Exceptions;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Domain.Models;

public class Cover
{
    [BsonId]
    public string Id { get; private set; }

    [BsonElement("startDate")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime StartDate { get; private set; }

    [BsonElement("endDate")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime EndDate { get; private set; }

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

    private Cover(string id, DateTime startDate, DateTime endDate, CoverType type, decimal premium)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new DomainValidationException("Id cannot be empty", nameof(id));
        }

        if(premium < 0)
        {
            throw new DomainValidationException("Premium must be zero or positive number", nameof(premium));
        }

        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Premium = premium;
    }

    public static Cover Create(DateTime startDate, DateTime endDate, CoverType type, decimal premium)
    {
        string id = Guid.NewGuid().ToString();
        return new Cover(id, startDate, endDate, type, premium);
    }
}
