using DivisionService.DAL.Models;

namespace DivisionService.DAL.Repository;

public interface IDivisionOperationRepository
{
    public Task RecordDivisionResult(DivisionOperationData divisionOperation);
}