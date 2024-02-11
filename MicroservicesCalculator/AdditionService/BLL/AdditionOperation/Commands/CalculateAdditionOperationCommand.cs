using Common.Shared.Abstractions.Messaging;
using Contracts;

namespace AdditionService.BLL.AdditionOperation.Commands;

public sealed record CalculateAdditionOperationCommand(decimal Operand1, decimal Operand2) : ICommand<CalculationResult>;