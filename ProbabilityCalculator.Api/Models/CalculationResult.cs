namespace ProbabilityCalculator.Api.Models;

public class CalculationResult
{
    public decimal Result { get; set; }
    public CalculationType CalculationType { get; set; }
    public decimal ProbabilityA { get; set; }
    public decimal ProbabilityB { get; set; }
}
