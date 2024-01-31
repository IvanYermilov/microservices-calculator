namespace AdditionService.BLL.AdditionService;

public interface IAdditionService
{
    public Task Plus(double operand1, double operand2);
}