using AdditionService.BLL.Activities.AdditionActivity;
using Contracts;
using DivisionService.BLL.Activities;
using MassTransit;
using MassTransit.Courier.Contracts;
using MultiplicationService.BLL.Activities;
using SubtractionService.BLL.Activities;

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
        Stack<decimal?> calculationStack = new Stack<decimal?>();
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        builder.AddVariable("ResultsStack", new Stack<decimal>());
        foreach (var stackValue in expressionAsStackPostfix)
        {
            if (decimal.TryParse(stackValue, out var operand))
            {
                calculationStack.Push(operand);
            }
            else
            {
                calculationStack.TryPop(out var operand2);
                calculationStack.TryPop(out var operand1); 

                switch (stackValue)
                {
                    case Constants.Plus:
                        builder.AddActivity("PlusActivity", new Uri($"exchange:{formatter.ExecuteActivity<AdditionActivity, OperationOperands>()}"),
                            new
                            {
                                Operand1 = operand1,
                                Operand2 = operand2,
                            });
                        calculationStack.Push(null);
                        break;
                    case Constants.Minus:
                        builder.AddActivity("MinusActivity", new Uri($"exchange:{formatter.ExecuteActivity<SubtractionActivity, OperationOperands>()}"),
                            new
                            {
                                Operand1 = operand1,
                                Operand2 = operand2,
                            });
                        calculationStack.Push(null);
                        break;
                    case Constants.Multiply:
                        builder.AddActivity("MultiplyActivity", new Uri($"exchange:{formatter.ExecuteActivity<MultiplicationActivity, OperationOperands>()}"),
                            new
                            {
                                Operand1 = operand1,
                                Operand2 = operand2,
                            });
                        calculationStack.Push(null);
                        break;
                    case Constants.Divide:
                        builder.AddActivity("DivideActivity", new Uri($"exchange:{formatter.ExecuteActivity<DivisionActivity, OperationOperands>()}"),
                            new
                            {
                                Operand1 = operand1,
                                Operand2 = operand2,
                            });
                        calculationStack.Push(null);
                        break;
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