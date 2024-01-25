using CalculatorAPI.BLL;
using CalculatorAPI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculateOperationController(ILogger<CalculateOperationController> logger, IExpressionManager expressionManager) : ControllerBase
{
    private IExpressionManager _expressionManager = expressionManager;

    private readonly ILogger<CalculateOperationController> _logger = logger;

    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateOperationAsync([FromBody]OperationData operationData, IValidator<OperationData> validator)
    {
        var validationResult = await validator.ValidateAsync(operationData);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var calculationResult = _expressionManager.Calculate(operationData.Expression!);

        return Ok($"{operationData.Expression} = {calculationResult}");
    }
}