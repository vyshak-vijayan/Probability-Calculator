namespace ProbabilityCalculator.Api.Models;

public class CalculationRequest
{
    public decimal ProbabilityA { get; set; }
    public decimal ProbabilityB { get; set; }
    public CalculationType CalculationType { get; set; }
}
