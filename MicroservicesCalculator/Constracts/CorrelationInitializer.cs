using System.Runtime.CompilerServices;
using MassTransit;

namespace Contracts;

public class CorrelationInitializer
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void Initialize()
    {
        MessageCorrelation.UseCorrelationId<ExpressionReceived>(x => x.ExpressionCalculationId);
        MessageCorrelation.UseCorrelationId<CalculateExpression>(x => x.ExpressionCalculationId);
        MessageCorrelation.UseCorrelationId<ConvertExpression>(x => x.ExpressionCalculationId);
        MessageCorrelation.UseCorrelationId<ConversionSucceed>(x => x.ExpressionCalculationId);
        MessageCorrelation.UseCorrelationId<ConversionFailed>(x => x.ExpressionCalculationId);
        MessageCorrelation.UseCorrelationId<CalculationSucceed>(x => x.ExpressionCalculationId);
        MessageCorrelation.UseCorrelationId<CalculationFailed>(x => x.ExpressionCalculationId);
    }
}