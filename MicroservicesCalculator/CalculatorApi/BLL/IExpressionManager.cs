namespace CalculatorAPI.BLL;

public interface IExpressionManager
{
    Task<decimal> Calculate(string expression, CancellationToken cancellationToken);
}