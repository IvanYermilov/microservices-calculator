namespace Contracts;

public record ExpressionCalculationDetails
{
    public Guid ExpressionCalculationId { get; init; }
    public string Expression { get; init; }
}