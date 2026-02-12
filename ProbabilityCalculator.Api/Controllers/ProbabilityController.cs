using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Services;

namespace ProbabilityCalculator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProbabilityController : ControllerBase
{
    private readonly IProbabilityService _probabilityService;
    private readonly IValidator<CalculationRequest> _validator;
    private readonly ILogger<ProbabilityController> _logger;

    public ProbabilityController(
        IProbabilityService probabilityService,
        IValidator<CalculationRequest> validator,
        ILogger<ProbabilityController> logger)
    {
        _probabilityService = probabilityService;
        _validator = validator;
        _logger = logger;
    }

    [HttpPost("calculate")]
    [ProducesResponseType(typeof(CalculationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] CalculationRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            _logger.LogWarning("Validation failed: {@Errors}", errors);

            return BadRequest(new ApiError
            {
                StatusCode = 400,
                Message = "Validation failed.",
                Detail = string.Join(" ", errors)
            });
        }

        var result = _probabilityService.Calculate(request);

        _logger.LogInformation(
            "{CalculationType} | P(A)={ProbabilityA} | P(B)={ProbabilityB} | Result={Result}",
            result.CalculationType,
            result.ProbabilityA,
            result.ProbabilityB,
            result.Result);

        return Ok(result);
    }
}
