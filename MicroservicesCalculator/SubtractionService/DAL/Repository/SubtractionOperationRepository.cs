using Common.Configurations;
using MongoDB.Driver;
using SubtractionService.DAL.Models;

namespace SubtractionService.DAL.Repository;

class SubtractionOperationRepository : ISubtractionOperationRepository
{
    private readonly IMongoCollection<SubtractionOperationData> _subtractionOperations;

    public SubtractionOperationRepository(IMongoClient mongoClient, MongoDbSettings mongoDbSettings)
    {
        var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        _subtractionOperations = database.GetCollection<SubtractionOperationData>(mongoDbSettings.CollectionName);
    }

    public async Task<Guid> RecordSubtractionResult(SubtractionOperationData subtractionOperationData)
    {
        await _subtractionOperations.InsertOneAsync(subtractionOperationData);

        return subtractionOperationData.Id;
    }

    public async Task RemoveSubtractionResult(Guid documentId)
    {
        await _subtractionOperations.DeleteOneAsync(so => so.Id == documentId);
    }
}