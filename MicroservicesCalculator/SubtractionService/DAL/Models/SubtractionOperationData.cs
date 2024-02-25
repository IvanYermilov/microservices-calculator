using MongoDB.Bson.Serialization.Attributes;

namespace SubtractionService.DAL.Models;

public class SubtractionOperationData
{
    [BsonId]
    public Guid Id { get; set; }

    public string? Expression { get; set; }

    public decimal Result { get; set; }
}