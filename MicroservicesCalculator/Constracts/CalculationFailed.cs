using MassTransit;

namespace Contracts;

public record CalculationFailed
{
    public Guid ExpressionCalculationId { get; init; }

    public ExceptionInfo ExceptionInfo { get; init; }
}