using Common.Shared.Abstractions.Messaging;
using Contracts;

namespace SubtractionService.BLL.SubtractionOperation.Commands;

public sealed record CalculateSubtractionOperationCommand(decimal Operand1, decimal Operand2) : ICommand<CalculationResult>;