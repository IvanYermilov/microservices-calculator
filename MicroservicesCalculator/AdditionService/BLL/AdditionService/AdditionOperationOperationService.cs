using AdditionService.DAL.Models;
using AdditionService.DAL.Repository;

namespace AdditionService.BLL.AdditionService;

class AdditionOperationOperationService(IAdditionOperationRepository additionOperationRepository) : IAdditionOperationService
{
    public async Task Plus(decimal operand1, decimal operand2)
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