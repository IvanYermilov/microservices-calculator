﻿using AdditionService.BLL.AdditionOperation.Commands;
using Contracts;
using MassTransit;
using MediatR;

namespace AdditionService.BLL.Activities.AdditionActivity;

public class AdditionActivity(ISender sender) : IActivity<OperationArguments, OperationCalculationLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<OperationArguments> context)
    {
        var command = new CalculateAdditionOperationCommand(context.Arguments.ResultOperand, context.Arguments.CalculationOperand);
        var result = await sender.Send(command);
        if (result.IsSuccess)
        {
            var log = new OperationCalculationLog()
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
        var command = new RemoveAdditionOperationResultCommand(context.Log.OperationId);
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            throw new Exception(result.Errors.ToString());
        }

        return context.Compensated();
    }
}