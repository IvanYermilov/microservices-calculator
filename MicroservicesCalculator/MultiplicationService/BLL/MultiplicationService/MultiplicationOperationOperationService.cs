using MultiplicationService.DAL.Models;
using MultiplicationService.DAL.Repository;

namespace MultiplicationService.BLL.MultiplicationService;

class MultiplicationOperationOperationService(IMultiplicationOperationRepository multiplicationOperationRepository) : IMultiplicationOperationService
{
    public async Task Multiply(decimal operand1, decimal operand2)
    {
        var multiplicationResult = operand1 + operand2;
        var multiplicationOperationData = new MultiplicationOperationData()
        {
            Expression = $"{operand1} * {operand2} = {multiplicationResult}",
            Result = multiplicationResult
        };

        await multiplicationOperationRepository.RecordMultiplicationResult(multiplicationOperationData);
    }
}