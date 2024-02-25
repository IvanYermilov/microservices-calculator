namespace Contracts;

public class ConversionSucceed
{
    public Guid ExpressionCalculationId { get; init; }
    public Stack<string> ExpressionAsStackPostfix { get; init; }
}