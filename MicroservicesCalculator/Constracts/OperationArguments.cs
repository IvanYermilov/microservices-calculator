namespace Contracts;

public record OperationArguments
{
    public decimal ResultOperand { get; init; }

    public decimal CalculationOperand { get; init; }
}