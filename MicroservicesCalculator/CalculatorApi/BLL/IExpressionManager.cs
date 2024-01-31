namespace CalculatorAPI.BLL;

public interface IExpressionManager
{
    Task<double> Calculate(string expression, CancellationToken cancellationToken);
}