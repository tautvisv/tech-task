using Claims.Domain.Exceptions;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Domain.Models;

public class CoverEntity
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
}
