using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdditionService.DAL.Models;

public class AdditionOperationData
{
    [BsonId]
    public Guid Id { get; set; }

    public string? Expression { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Result { get; set; }
}