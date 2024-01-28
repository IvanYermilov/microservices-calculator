using CalculatorAPI.BLL;
using CalculatorAPI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController(ILogger<CalculatorController> logger, IExpressionManager expressionManager) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateOperationAsync([FromBody]OperationData operationData, IValidator<OperationData> validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(operationData, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var calculationResult = await expressionManager.Calculate(operationData.Expression!, cancellationToken);

        return Ok($"{operationData.Expression} = {calculationResult}");
    }
}