namespace Contracts;

public record SubtractEvent
{
    public decimal Operand1 { get; init; } = 0;
    public decimal Operand2 { get; init; } = 0;
}