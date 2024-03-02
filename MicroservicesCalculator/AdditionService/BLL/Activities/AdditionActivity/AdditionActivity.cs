using AdditionService.BLL.AdditionOperation.Commands;
using Common.Extensions;
using Contracts;
using MassTransit;
using MediatR;

namespace AdditionService.BLL.Activities.AdditionActivity;

public class AdditionActivity(ISender sender) : IActivity<OperationOperands, OperationCalculationLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<OperationOperands> context)
    {
        var resultsStack = new Stack<decimal>(((List<object>)context.Message.Variables["ResultsStack"]).Select(Convert.ToDecimal).ToList());
        var operands = context.GetOperands(resultsStack);
        var command = new CalculateAdditionOperationCommand(operands.Operand1!.Value, operands.Operand2!.Value);
        var result = await sender.Send(command);
        if (result.IsSuccess)
        {
            var log = new OperationCalculationLog
            {
                OperationId = result.Value.CalculationResultId
            };
            resultsStack.Push(result.Value.Result);

            return context.CompletedWithVariables(log, new { ResultsStack = resultsStack });
        }

        throw new Exception(result.Errors.ToString());
    }

    public async Task<CompensationResult> Compensate(CompensateContext<OperationCalculationLog> context)
    {
        var command = new RemoveAdditionOperationResultCommand(context.Log.OperationId);
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            throw new Exception(result.Errors.ToString());
        }

        return context.Compensated();
    }
}