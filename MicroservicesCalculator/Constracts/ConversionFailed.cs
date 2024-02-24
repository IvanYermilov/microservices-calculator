namespace Contracts;

public record ConversionFailed
{
    public Guid ExpressionCalculationId { get; init; }

    public Exception Exception { get; init; }
}