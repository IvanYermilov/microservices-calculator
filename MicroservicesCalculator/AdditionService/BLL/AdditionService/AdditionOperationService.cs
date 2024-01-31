using AdditionService.DAL.Models;
using AdditionService.DAL.Repository;

namespace AdditionService.BLL.AdditionService;

class AdditionOperationService(IAdditionOperationRepository additionOperationRepository) : IAdditionService
{
    public async Task Plus(double operand1, double operand2)
    {
        var additionResult = operand1 + operand2;
        var additionOperationData = new AdditionOperationData()
        {
            Expression = $"{operand1} + {operand2} = {additionResult}",
            Result = additionResult
        };

        await additionOperationRepository.RecordAdditionResult(additionOperationData);
    }
}