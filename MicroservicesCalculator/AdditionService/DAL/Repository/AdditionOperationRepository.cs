using AdditionService.DAL.Models;
using Common.Configurations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AdditionService.DAL.Repository;

class AdditionOperationRepository : IAdditionOperationRepository
{
    private readonly IMongoCollection<AdditionOperationData> _additionOperations;

    public AdditionOperationRepository(IMongoClient mongoClient, MongoDbSettings mongoDbSettings)
    {
        var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        _additionOperations = database.GetCollection<AdditionOperationData>(mongoDbSettings.CollectionName);
    }

    public async Task<Guid> RecordAdditionResult(AdditionOperationData additionOperation)
    {
        await _additionOperations.InsertOneAsync(additionOperation);

        return additionOperation.Id;

        //if (Guid.TryParse(additionOperation.Id, out Guid id))
        //{
        //    return id;
        //}
        //else
        //{
        //    return Guid.Empty;
        //}
    }

    public async Task RemoveAdditionResult(Guid documentId)
    {
        await _additionOperations.DeleteOneAsync(ao => ao.Id == documentId);
    }
}