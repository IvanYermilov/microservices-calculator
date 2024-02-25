using Common.Shared.Abstractions.Messaging;
using Contracts;

namespace DivisionService.BLL.MultiplicationOperation.Commands;

public sealed record CalculateDivisionOperationCommand(decimal Operand1, decimal Operand2) : ICommand<CalculationResult>;