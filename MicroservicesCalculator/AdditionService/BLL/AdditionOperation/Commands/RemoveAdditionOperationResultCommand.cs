using Common.Shared.Abstractions.Messaging;

namespace AdditionService.BLL.AdditionOperation.Commands;

public sealed record RemoveAdditionOperationResultCommand(Guid AdditionOperationResultId) : ICommand;
