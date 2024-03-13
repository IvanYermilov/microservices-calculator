using Common.Configurations;
using DivisionService.DAL.Models;
using MongoDB.Driver;

namespace DivisionService.DAL.Repository;

class DivisionOperationRepository : IDivisionOperationRepository
{
    private readonly IMongoCollection<DivisionOperationData> _divisionOperations;

    public DivisionOperationRepository(IMongoClient mongoClient, MongoDbSettings mongoDbSettings)
    {
        var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        _divisionOperations = database.GetCollection<DivisionOperationData>(mongoDbSettings.CollectionName);
    }

    public async Task<Guid> RecordDivisionResult(DivisionOperationData divisionOperation)
    {
        await _divisionOperations.InsertOneAsync(divisionOperation);
        return divisionOperation.Id;
    }

    public async Task RemoveDivisionResult(Guid documentId)
    {
        await _divisionOperations.DeleteOneAsync(divo => divo.Id == documentId);
    }
}