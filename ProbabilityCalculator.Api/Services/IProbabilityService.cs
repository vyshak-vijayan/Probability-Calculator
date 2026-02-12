using ProbabilityCalculator.Api.Models;

namespace ProbabilityCalculator.Api.Services;

public interface IProbabilityService
{
    CalculationResult Calculate(CalculationRequest request);
}
