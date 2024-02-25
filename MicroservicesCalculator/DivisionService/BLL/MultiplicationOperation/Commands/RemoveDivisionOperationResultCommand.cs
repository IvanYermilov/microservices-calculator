using Common.Shared.Abstractions.Messaging;

namespace DivisionService.BLL.MultiplicationOperation.Commands;

public sealed record RemoveDivisionOperationResultCommand(Guid DivisionOperationResultId) : ICommand;
