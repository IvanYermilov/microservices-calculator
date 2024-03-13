using DivisionService.DAL.Models;

namespace DivisionService.DAL.Repository;

public interface IDivisionOperationRepository
{
    public Task<Guid> RecordDivisionResult(DivisionOperationData divisionOperation);
    public Task RemoveDivisionResult(Guid documentId);
}