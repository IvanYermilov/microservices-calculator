using Contracts;
using MassTransit;
using MediatR;
using SubtractionService.BLL.SubtractionOperation.Commands;

namespace SubtractionService.BLL.Activities;

public class SubtractionActivity(ISender sender) : IActivity<OperationArguments, OperationCalculationLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<OperationArguments> context)
    {
        var command = new CalculateSubtractionOperationCommand(context.Arguments.ResultOperand, context.Arguments.CalculationOperand);
        var result = await sender.Send(command);
        if (result.IsSuccess)
        {
            var log = new OperationCalculationLog
            {
                OperationId = result.Value.CalculationResultId
            };
            context.Message.Variables["ResultOperand"] = result.Value.Result;
            return context.CompletedWithVariables(log, result.Value.Result);
        }

        throw new Exception(result.Errors.ToString());
    }

    public async Task<CompensationResult> Compensate(CompensateContext<OperationCalculationLog> context)
    {
        var command = new RemoveSubtractionOperationResultCommand(context.ExecutionId);
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            throw new Exception(result.Errors.ToString());
        }

        return context.Compensated();
    }
}