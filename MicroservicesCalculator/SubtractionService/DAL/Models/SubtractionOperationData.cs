using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SubtractionService.DAL.Models;

public class SubtractionOperationData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string? Expression { get; set; }

    public decimal Result { get; set; }
}