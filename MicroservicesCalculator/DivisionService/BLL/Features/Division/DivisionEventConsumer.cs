using Contracts;
using DivisionService.BLL.DivisionService;
using MassTransit;

namespace DivisionService.BLL.Features.Division;

public sealed class DivisionEventConsumer(IDivisionService divisionService) : IConsumer<DivideEvent>
{
    public async Task Consume(ConsumeContext<DivideEvent> context)
    {
        var message = context.Message;
        await divisionService.Divide(message.Operand1, message.Operand2);
    }
}