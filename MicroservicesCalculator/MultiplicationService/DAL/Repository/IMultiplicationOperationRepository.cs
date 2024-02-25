using MultiplicationService.DAL.Models;

namespace MultiplicationService.DAL.Repository;

public interface IMultiplicationOperationRepository
{
    public Task<Guid> RecordMultiplicationResult(MultiplicationOperationData multiplicationOperation);

    public Task RemoveMultiplicationResult(Guid documentId);
}