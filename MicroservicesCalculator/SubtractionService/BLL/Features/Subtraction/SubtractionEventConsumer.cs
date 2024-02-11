using Contracts;
using MassTransit;
using SubtractionService.BLL.SubtractionService;

namespace SubtractionService.BLL.Features.Subtraction;

public sealed class SubtractionEventConsumer(ISubtractionOperationService subtractionService) : IConsumer<SubtractEvent>
{
    public async Task Consume(ConsumeContext<SubtractEvent> context)
    {
        var message = context.Message;
        await subtractionService.Subtract(message.Operand1, message.Operand2);
    }
}