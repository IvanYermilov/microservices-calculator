using MongoDB.Bson.Serialization.Attributes;

namespace DivisionService.DAL.Models;

public class DivisionOperationData
{
    [BsonId]
    public Guid Id { get; set; }

    public string? Expression { get; set; }

    public decimal Result { get; set; }
}