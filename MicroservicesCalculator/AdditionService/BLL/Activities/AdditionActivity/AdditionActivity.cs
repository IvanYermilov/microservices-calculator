using AdditionService.BLL.AdditionOperation.Commands;
using Contracts;
using MassTransit;
using MediatR;

namespace AdditionService.BLL.Activities.AdditionActivity;

public class AdditionActivity(ISender sender) : IActivity<OperationArguments, OperationCalculationLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<OperationArguments> context)
    {
        var command = new CalculateAdditionOperationCommand(context.Arguments.Operand1, context.Arguments.Operand2);
        var result = await sender.Send(command);
        var log = new OperationCalculationLog()
        {
            OperationId = result.Value.CalculationResultId
        };
        context.Message.Variables["Operand1"] = result.Value.Result;
        return context.CompletedWithVariables(log,result.Value.Result);
    }

    public async Task<CompensationResult> Compensate(CompensateContext<OperationCalculationLog> context)
    {
        var command = new RemoveAdditionOperationResultCommand(context.Log.OperationId);
        var deletionResult = await sender.Send(command);
        if (!deletionResult.IsSuccess)
        {
            throw new Exception(deletionResult.Errors.FirstOrDefault());
        }

        return context.Compensated();
    }
}