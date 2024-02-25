using Common.Shared.Abstractions.Messaging;

namespace SubtractionService.BLL.SubtractionOperation.Commands;

public sealed record RemoveSubtractionOperationResultCommand(Guid SubtractionOperationResultId) : ICommand;
