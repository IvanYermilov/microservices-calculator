using Contracts;
using MassTransit;
using MultiplicationService.BLL.MultiplicationService;

namespace MultiplicationService.BLL.Features.Multiplication;

public sealed class MultiplicationEventConsumer(IMultiplicationOperationService multiplicationOperationService) : IConsumer<MultiplyEvent>
{
    public async Task Consume(ConsumeContext<MultiplyEvent> context)
    {
        var message = context.Message;
        await multiplicationOperationService.Multiply(message.Operand1, message.Operand2);
    }
}