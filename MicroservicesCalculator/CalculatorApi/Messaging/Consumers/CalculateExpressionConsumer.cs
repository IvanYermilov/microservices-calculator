using AdditionService.BLL.Activities.AdditionActivity;
using Contracts;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace CalculatorAPI.Messaging.Consumers;

public sealed class CalculateExpressionConsumer(IEndpointNameFormatter formatter) : IConsumer<CalculateExpression>
{
    public async Task Consume(ConsumeContext<CalculateExpression> context)
    {
        try
        {
             await CreateRoutingSlip(context.Message.ExpressionAsStackPostfix, context, context.CancellationToken);
        }
        catch (Exception e)
        {
            var calculationFailedMessage= new CalculationFailed
            {
                ExpressionCalculationId = context.Message.ExpressionCalculationId,
            };
            await context.Publish(calculationFailedMessage, context.CancellationToken);
        }
    }


    private async Task CreateRoutingSlip(Stack<string> expressionAsStackPostfix, ConsumeContext<CalculateExpression> context, CancellationToken cancellationToken)
    {
        bool isFirstOperation = true;
        Stack<decimal> calculationStack = new Stack<decimal>();
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        foreach (var stackValue in expressionAsStackPostfix)
        {
            if (decimal.TryParse(stackValue, out var operand))
            {
                calculationStack.Push(operand);
            }
            else
            {
                
                if (isFirstOperation)
                {
                    var operand1 = calculationStack.Pop();
                    builder.AddVariable("Operand1", operand1);

                    isFirstOperation = false;
                }

                var operand2 = calculationStack.Pop();

                switch (stackValue)
                {
                    case Constants.Plus:
                        //calculationStack.Push(operand2 + operand1);
                        //await bus.Publish(new PlusEvent()
                        //{
                        //    Operand1 = operand1,
                        //    Operand2 = operand2
                        //}, cancellationToken);
                        builder.AddActivity("PlusActivity", new Uri($"exchange:{formatter.ExecuteActivity<AdditionActivity, OperationArguments>()}"),
                            new
                            {
                                Operand2 = operand2
                            });
                        break;
                    //case Constants.Minus:
                    //    calculationStack.Push(operand2 - operand1);
                    //    break;
                    //case Constants.Multiply:
                    //    calculationStack.Push(operand2 * operand1);
                    //    break;
                    //case Constants.Divide:
                    //    calculationStack.Push(operand2 / operand1);
                    //    break;
                }
            }
        }

        builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.Completed, RoutingSlipEventContents.Variables,
            x => x.Send<CalculationSucceed>(new { context.Message.ExpressionCalculationId }, cancellationToken));
        builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.ActivityFaulted, RoutingSlipEventContents.All,
            x => x.Send<CalculationFailed>(new { context.Message.ExpressionCalculationId }, cancellationToken));

        var routingSlip = builder.Build();

        await context.Execute(routingSlip, cancellationToken);
    }
}