using Common.Shared.Abstractions.Messaging;
using Contracts;

namespace MultiplicationService.BLL.MultiplicationOperation.Commands;

public sealed record CalculateMultiplicationOperationCommand(decimal Operand1, decimal Operand2) : ICommand<CalculationResult>;