using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DivisionService.DAL.Models;

public class DivisionOperationData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string? Expression { get; set; }

    public decimal Result { get; set; }
}