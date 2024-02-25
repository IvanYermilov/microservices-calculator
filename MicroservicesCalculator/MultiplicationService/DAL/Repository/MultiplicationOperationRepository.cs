using Common.Configurations;
using MongoDB.Driver;
using MultiplicationService.DAL.Models;

namespace MultiplicationService.DAL.Repository;

class MultiplicationOperationRepository : IMultiplicationOperationRepository
{
    private readonly IMongoCollection<MultiplicationOperationData> _multiplicationOperations;

    public MultiplicationOperationRepository(IMongoClient mongoClient, MongoDbSettings mongoDbSettings)
    {
        var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        _multiplicationOperations = database.GetCollection<MultiplicationOperationData>(mongoDbSettings.CollectionName);
    }

    public async Task<Guid> RecordMultiplicationResult(MultiplicationOperationData multiplicationOperation)
    {
        await _multiplicationOperations.InsertOneAsync(multiplicationOperation);
        return multiplicationOperation.Id;
    }

    public async Task RemoveMultiplicationResult(Guid documentId)
    {
        await _multiplicationOperations.DeleteOneAsync(mo => mo.Id == documentId);
    }
}