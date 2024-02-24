namespace CalculatorAPI.BLL;

public interface IExpressionManager
{
    Task<Stack<string>> Convert(string expression, CancellationToken cancellationToken);
}