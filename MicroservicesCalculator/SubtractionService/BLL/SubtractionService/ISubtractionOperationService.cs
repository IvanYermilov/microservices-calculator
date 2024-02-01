namespace SubtractionService.BLL.SubtractionService;

public interface ISubtractionOperationService
{
    public Task Subtract(decimal operand1, decimal operand2);
}