 namespace Contracts;

 public record PlusEvent
 {
     public decimal Operand1 { get; init; } = 0;
     public decimal Operand2 { get; init; } = 0;
 }