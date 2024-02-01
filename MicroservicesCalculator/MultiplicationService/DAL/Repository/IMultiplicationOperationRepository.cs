using MultiplicationService.DAL.Models;

namespace MultiplicationService.DAL.Repository;

public interface IMultiplicationOperationRepository
{
    public Task RecordMultiplicationResult(MultiplicationOperationData multiplicationOperation);
}