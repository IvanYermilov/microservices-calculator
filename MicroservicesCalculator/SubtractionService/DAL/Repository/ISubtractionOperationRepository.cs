using SubtractionService.DAL.Models;

namespace SubtractionService.DAL.Repository;

public interface ISubtractionOperationRepository
{
    public Task RecordSubtractionResult(SubtractionOperationData subtractionOperation);
}