using AdditionService.BLL.AdditionService;
using Contracts;
using MassTransit;

namespace AdditionService.BLL.Features.Addition;

public sealed class AdditionEventConsumer(IAdditionService additionService) : IConsumer<PlusEvent>
{
    public async Task Consume(ConsumeContext<PlusEvent> context)
    {
        var message = context.Message;
        await additionService.Plus(message.Operand1, message.Operand2);
    }
}