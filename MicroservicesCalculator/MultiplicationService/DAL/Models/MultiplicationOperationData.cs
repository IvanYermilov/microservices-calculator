using MongoDB.Bson.Serialization.Attributes;

namespace MultiplicationService.DAL.Models;

public class MultiplicationOperationData
{
    [BsonId]
    public Guid Id { get; set; }

    public string? Expression { get; set; }

    public decimal Result { get; set; }
}