using SubtractionService.DAL.Models;
using SubtractionService.DAL.Repository;

namespace SubtractionService.BLL.SubtractionService;

class SubtractionOperationService(ISubtractionOperationRepository subtractionOperationRepository) : ISubtractionOperationService
{
    public async Task Subtract(decimal operand1, decimal operand2)
    {
        var subtractionResult = operand1 - operand2;
        var subtractionOperationData = new SubtractionOperationData()
        {
            Expression = $"{operand1} - {operand2} = {subtractionResult}",
            Result = subtractionResult
        };

        await subtractionOperationRepository.RecordSubtractionResult(subtractionOperationData);
    }
}