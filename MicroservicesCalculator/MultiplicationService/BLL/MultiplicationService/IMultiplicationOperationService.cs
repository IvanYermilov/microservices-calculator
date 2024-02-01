namespace MultiplicationService.BLL.MultiplicationService;

public interface IMultiplicationOperationService
{
    public Task Multiply(decimal operand1, decimal operand2);
}