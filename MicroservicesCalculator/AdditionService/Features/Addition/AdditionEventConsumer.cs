using Contracts;
using MassTransit;

namespace AdditionService.Features.Addition;

public sealed class AdditionEventConsumer : IConsumer<PlusEvent>
{
    public Task Consume(ConsumeContext<PlusEvent> context)
    {
        var a = context.Message;
        return Task.CompletedTask;
    }
}