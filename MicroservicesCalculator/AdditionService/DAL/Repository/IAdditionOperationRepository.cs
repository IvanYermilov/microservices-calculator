using AdditionService.DAL.Models;

namespace AdditionService.DAL.Repository;

public interface IAdditionOperationRepository
{
    public Task<Guid> RecordAdditionResult(AdditionOperationData additionOperation);

    public Task RemoveAdditionResult(Guid documentId);
}