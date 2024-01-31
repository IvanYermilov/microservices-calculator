using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdditionService.DAL.Models;

public class AdditionOperationData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string? Expression { get; set; }

    public double Result { get; set; }
}