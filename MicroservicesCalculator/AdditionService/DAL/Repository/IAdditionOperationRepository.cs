using AdditionService.DAL.Models;

namespace AdditionService.DAL.Repository;

public interface IAdditionOperationRepository
{
    public Task RecordAdditionResult(AdditionOperationData additionOperation);
}