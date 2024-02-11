using AdditionService.DAL.Models;
using AdditionService.DAL.Repository;
using Common.Shared;
using Common.Shared.Abstractions.Messaging;
using Contracts;

namespace AdditionService.BLL.AdditionOperation.Commands;

public class CalculateAdditionOperationCommandHandler(IAdditionOperationRepository additionOperationRepository) : ICommandHandler<CalculateAdditionOperationCommand, CalculationResult>
{
    public async Task<Result<CalculationResult>> Handle(CalculateAdditionOperationCommand request, CancellationToken cancellationToken)
    {
        var additionResult = request.Operand1 + request.Operand2;
        var additionOperationData = new AdditionOperationData()
        {
            Expression = $"{request.Operand1} + {request.Operand2} = {additionResult}",
            Result = additionResult
        };

        var result = new Result<CalculationResult>();
        result.Value = new CalculationResult();

        try
        {
            var calculationResultId = await additionOperationRepository.RecordAdditionResult(additionOperationData);
            result.Value.Result = additionResult;
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