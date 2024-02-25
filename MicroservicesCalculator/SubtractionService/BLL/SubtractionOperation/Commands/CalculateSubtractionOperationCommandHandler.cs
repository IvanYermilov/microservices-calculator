using Common.Shared;
using Common.Shared.Abstractions.Messaging;
using Contracts;
using SubtractionService.DAL.Models;
using SubtractionService.DAL.Repository;

namespace SubtractionService.BLL.SubtractionOperation.Commands;

public class CalculateSubtractionOperationCommandHandler(ISubtractionOperationRepository subtractionOperationRepository) : ICommandHandler<CalculateSubtractionOperationCommand, CalculationResult>
{
    public async Task<Result<CalculationResult>> Handle(CalculateSubtractionOperationCommand request, CancellationToken cancellationToken)
    {
        var subtractionResult = request.Operand1 - request.Operand2;
        var subtractionOperationData = new SubtractionOperationData
        {
            Expression = $"{request.Operand1} - {request.Operand2} = {subtractionResult}",
            Result = subtractionResult
        };

        var result = new Result<CalculationResult>
        {
            Value = new CalculationResult()
        };

        try
        {
            var calculationResultId = await subtractionOperationRepository.RecordSubtractionResult(subtractionOperationData);
            result.Value.Result = subtractionResult;
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