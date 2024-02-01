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

    public async Task RecordDivisionResult(DivisionOperationData divisionOperation)
    {
        await _divisionOperations.InsertOneAsync(divisionOperation);
    }
}