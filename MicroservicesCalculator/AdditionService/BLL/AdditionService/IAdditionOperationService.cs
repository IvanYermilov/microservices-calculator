namespace AdditionService.BLL.AdditionService;

public interface IAdditionOperationService
{
    public Task Plus(decimal operand1, decimal operand2);
}