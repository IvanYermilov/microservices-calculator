using CalculatorAPI.BLL;
using CalculatorAPI.Models;
using Contracts;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController(ILogger<CalculatorController> logger) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateOperationAsync([FromBody]OperationData operationData, IValidator<OperationData> validator, CancellationToken cancellationToken, [FromServices] IPublishEndpoint publishEndpoint)
    {
        var validationResult = await validator.ValidateAsync(operationData, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var message = new ExpressionReceived()
        {
            ExpressionCalculationId = operationData.OperationId,
            Expression = operationData.Expression,
        };

        publishEndpoint.Publish(message, cancellationToken);

        return Ok($"Expression \"{operationData.Expression}\" was accepted. Calculation ID is {operationData.OperationId}");
    }
}