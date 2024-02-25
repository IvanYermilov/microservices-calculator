using Common.Shared.Abstractions.Messaging;

namespace MultiplicationService.BLL.MultiplicationOperation.Commands;

public sealed record RemoveMultiplicationOperationResultCommand(Guid MultiplicationOperationResultId) : ICommand;
