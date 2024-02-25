using MassTransit;

namespace CalculatorAPI.Messaging.StateMachines;

public class CalculateExpressionState
    : SagaStateMachineInstance, ISagaVersion
{
    public string Expression { get; set; } = string.Empty;
    public decimal Result { get; set; }
    public string? CurrentState { get; set; }
    public int RetryAttempt { get; set; }
    public Guid? ScheduleRetryToken { get; set; }
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
}