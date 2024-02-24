using CalculatorAPI.BLL;
using Contracts;
using MassTransit;

namespace CalculatorAPI.Messaging.Consumers;

public sealed class ConvertExpressionConsumer(IExpressionManager expressionManager) : IConsumer<ConvertExpression>
{
    public async Task Consume(ConsumeContext<ConvertExpression> context)
    {
        try
        {
            var expressionAsStackPostfix = await expressionManager.Convert(context.Message.Expression, context.CancellationToken);

            var conversionSucceedMessage = new ConversionSucceed
            {
                ExpressionCalculationId = context.Message.ExpressionCalculationId,
                ExpressionAsStackPostfix = expressionAsStackPostfix,
            };

            context.Publish(conversionSucceedMessage);
        }
        catch (Exception e)
        {
            var conversionFailedMessage = new ConversionFailed
            {
                Exception = e,
                ExpressionCalculationId = context.Message.ExpressionCalculationId,
            };
            context.Publish(conversionFailedMessage, context.CancellationToken);
        }
    }
}