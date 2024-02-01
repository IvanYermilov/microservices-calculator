using AdditionService.DAL.Models;
using Common.Configurations;
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

    public async Task RecordAdditionResult(AdditionOperationData additionOperation)
    {
        await _additionOperations.InsertOneAsync(additionOperation);
    }
}