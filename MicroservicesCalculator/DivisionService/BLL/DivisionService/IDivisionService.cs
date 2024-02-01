namespace DivisionService.BLL.DivisionService;

public interface IDivisionService
{
    public Task Divide(decimal operand1, decimal operand2);
}