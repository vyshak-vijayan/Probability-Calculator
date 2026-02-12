using ProbabilityCalculator.Api.Models;

namespace ProbabilityCalculator.Api.Services;

public class ProbabilityService : IProbabilityService
{
    public CalculationResult Calculate(CalculationRequest request)
    {
        var result = request.CalculationType switch
        {
            CalculationType.CombinedWith => CombinedWith(request.ProbabilityA, request.ProbabilityB),
            CalculationType.Either => Either(request.ProbabilityA, request.ProbabilityB),
            _ => throw new ArgumentOutOfRangeException(
                     nameof(request.CalculationType),
                     $"Unknown calculation type: {request.CalculationType}")
        };

        return new CalculationResult
        {
            Result = result,
            CalculationType = request.CalculationType,
            ProbabilityA = request.ProbabilityA,
            ProbabilityB = request.ProbabilityB
        };
    }

    private static decimal CombinedWith(decimal a, decimal b) => a * b;

    private static decimal Either(decimal a, decimal b) => a + b - (a * b);
}
