using CalculatorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculateOperationController(ILogger<CalculateOperationController> logger) : ControllerBase
{

    private readonly ILogger<CalculateOperationController> _logger = logger;

    [HttpPost("Calculate")]
    public IActionResult CalculateOperationAsync([FromBody]OperationData? operationData)
    {
        if (operationData is null)
        {
            return BadRequest("Na data in body");
        }

        return Ok("Nice");
    }
}