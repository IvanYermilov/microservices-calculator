using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiplicationService.DAL.Models;

public class MultiplicationOperationData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string? Expression { get; set; }

    public decimal Result { get; set; }
}