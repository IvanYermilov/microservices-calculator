using DivisionService.DAL.Models;
using DivisionService.DAL.Repository;

namespace DivisionService.BLL.DivisionService;

class DivisionOperationService(IDivisionOperationRepository divisionOperationRepository) : IDivisionService
{
    public async Task Divide(decimal operand1, decimal operand2)
    {
        var divisionResult = operand1 / operand2;
        var divisionOperationData = new DivisionOperationData()
        {
            Expression = $"{operand1} / {operand2} = {divisionResult}",
            Result = divisionResult
        };

        await divisionOperationRepository.RecordDivisionResult(divisionOperationData);
    }
}