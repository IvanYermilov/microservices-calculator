using SubtractionService.DAL.Models;

namespace SubtractionService.DAL.Repository;

public interface ISubtractionOperationRepository
{
    public Task<Guid> RecordSubtractionResult(SubtractionOperationData subtractionOperation);
    
    public Task RemoveSubtractionResult(Guid documentId);
}