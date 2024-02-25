using Common.Shared;
using Common.Shared.Abstractions.Messaging;
using Contracts;
using DivisionService.DAL.Models;
using DivisionService.DAL.Repository;

namespace DivisionService.BLL.MultiplicationOperation.Commands;

public class CalculateDivisionOperationCommandHandler(IDivisionOperationRepository divisionOperationRepository) : ICommandHandler<CalculateDivisionOperationCommand, CalculationResult>
{
    public async Task<Result<CalculationResult>> Handle(CalculateDivisionOperationCommand request, CancellationToken cancellationToken)
    {
        var divisionResult = request.Operand1 / request.Operand2;
        var divisionOperationData = new DivisionOperationData
        {
            Expression = $"{request.Operand1} / {request.Operand2} = {divisionResult}",
            Result = divisionResult
        };

        var result = new Result<CalculationResult>
        {
            Value = new CalculationResult()
        };

        try
        {
            var calculationResultId = await divisionOperationRepository.RecordDivisionResult(divisionOperationData);
            result.Value.Result = divisionResult;
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