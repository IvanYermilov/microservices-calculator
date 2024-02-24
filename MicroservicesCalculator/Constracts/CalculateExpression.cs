namespace Contracts;

public record CalculateExpression
{
    public Guid ExpressionCalculationId { get; init; }
    public Stack<string> ExpressionAsStackPostfix { get; init; }
}