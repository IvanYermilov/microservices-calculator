using Contracts;
using MassTransit;

namespace CalculatorAPI.Messaging.StateMachines;

public class CalculateExpressionStateMachine : MassTransitStateMachine<CalculateExpressionState>
{
    public CalculateExpressionStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Initially(
            When(EventExpressionReceived)
                .Initialize()
                .Converting()
                .TransitionTo(Converted));

        During(Converted,
            When(EventConversionFailed)
                .TransitionTo(Suspended),
            When(EventConversionSucceed)
                .Calculate()
                .TransitionTo(Calculating));

        During(Calculating,
            When(EventCalculationFailed)
                .Kek()
                .TransitionTo(Suspended),
            When(EventCalculationSucceed)
                .Calculated()
                .TransitionTo(Calculated));

        During(Suspended,
            When(EventExpressionReceived)
                .Initialize()
                .Converting()
                .TransitionTo(Converted));
    }

    public State Converted { get; }
    public State Calculating { get; }
    public State Calculated { get; }
    public State Suspended { get; }

    public Event<ExpressionReceived> EventExpressionReceived { get; }
    public Event<ConversionFailed> EventConversionFailed { get; }
    public Event<ConversionSucceed> EventConversionSucceed { get; }
    public Event<CalculationSucceed> EventCalculationSucceed { get; }
    public Event<CalculationFailed> EventCalculationFailed { get; }
}

static class RegistrationStateMachineBehaviorExtensions
{
    public static EventActivityBinder<CalculateExpressionState, ExpressionReceived> Initialize(
        this EventActivityBinder<CalculateExpressionState, ExpressionReceived> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.Expression = context.Message.Expression;
        });
    }

    public static EventActivityBinder<CalculateExpressionState, ExpressionReceived> Converting(
        this EventActivityBinder<CalculateExpressionState, ExpressionReceived> binder)
    {
        Thread.Sleep(5000);
        return binder.PublishAsync(context => context.Init<ConvertExpression>(context.Message));
    }

    public static EventActivityBinder<CalculateExpressionState, ConversionSucceed> Calculate(
        this EventActivityBinder<CalculateExpressionState, ConversionSucceed> binder)
    {
        return binder.PublishAsync(context => context.Init<CalculateExpression>(context.Message));
    }

    public static EventActivityBinder<CalculateExpressionState, CalculationSucceed> Calculated(
        this EventActivityBinder<CalculateExpressionState, CalculationSucceed> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.Result = context.GetVariable<decimal>("ResultOperand") ?? 0;
        });
    }


    public static EventActivityBinder<CalculateExpressionState, CalculationFailed> Kek(
        this EventActivityBinder<CalculateExpressionState, CalculationFailed> binder)
    {
        return binder.Then(context =>
        {
            Console.WriteLine(context.Saga.Result);
        });
    }
}