using Common.Shared;
using Common.Shared.Abstractions.Messaging;
using Contracts;
using MultiplicationService.DAL.Models;
using MultiplicationService.DAL.Repository;

namespace MultiplicationService.BLL.MultiplicationOperation.Commands;

public class CalculateMultiplicationOperationCommandHandler(IMultiplicationOperationRepository multiplicationOperationRepository) : ICommandHandler<CalculateMultiplicationOperationCommand, CalculationResult>
{
    public async Task<Result<CalculationResult>> Handle(CalculateMultiplicationOperationCommand request, CancellationToken cancellationToken)
    {
        var multiplicationResult = request.Operand1 * request.Operand2;
        var multiplicationOperationData = new MultiplicationOperationData
        {
            Expression = $"{request.Operand1} * {request.Operand2} = {multiplicationResult}",
            Result = multiplicationResult
        };

        var result = new Result<CalculationResult>
        {
            Value = new CalculationResult()
        };

        try
        {
            var calculationResultId = await multiplicationOperationRepository.RecordMultiplicationResult(multiplicationOperationData);
            result.Value.Result = multiplicationResult;
            result.Value.CalculationResultId = calculationResultId;
            result.IsSuccess = true;
            return result;
        }
        catch (Exception e)
        {
            result.IsSuccess = false;
            result.Errors.Add(e.Message);
            return result;
        }
    }
}