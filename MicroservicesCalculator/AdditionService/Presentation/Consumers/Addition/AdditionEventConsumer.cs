using Contracts;
using MassTransit;
using MediatR;
using AdditionService.BLL.AdditionOperation.Commands;

namespace AdditionService.Presentation.Consumers.Addition;

public sealed class AdditionEventConsumer(ISender sender) : IConsumer<PlusEvent>
{
    public async Task Consume(ConsumeContext<PlusEvent> context)
    {
        var message = context.Message;
        var command = new CalculateAdditionOperationCommand(message.Operand1, message.Operand2);
        var result = await sender.Send(command);
    }
}