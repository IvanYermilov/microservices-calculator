using Contracts;
using MassTransit;

namespace Common.Extensions;

public static class RoutingSlipExecuteContext
{
    public static OperationOperands GetOperands(this ExecuteContext<OperationOperands> context, Stack<decimal> resultsStack)
    {
        return new()
        {
            Operand1 = context.Arguments.Operand1 ?? resultsStack.Pop(),
            Operand2 = context.Arguments.Operand2 ?? resultsStack.Pop()
        };
    }
}