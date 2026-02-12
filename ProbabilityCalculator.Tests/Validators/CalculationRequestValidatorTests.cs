using FluentAssertions;
using FluentValidation.TestHelper;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Validators;

namespace ProbabilityCalculator.Tests.Validators;

public class CalculationRequestValidatorTests
{
    private readonly CalculationRequestValidator _validator = new();

    [Theory]
    [InlineData(0, 0.5)]
    [InlineData(0.5, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0.333, 0.667)]
    public void Validate_ValidProbabilities_ShouldPass(double a, double b)
    {
        var request = new CalculationRequest
        {
            ProbabilityA = (decimal)a,
            ProbabilityB = (decimal)b,
            CalculationType = CalculationType.CombinedWith
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_NegativeProbabilityA_ShouldFail()
    {
        var request = new CalculationRequest
        {
            ProbabilityA = -0.1m,
            ProbabilityB = 0.5m,
            CalculationType = CalculationType.Either
        };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.ProbabilityA);
    }

    [Fact]
    public void Validate_ProbabilityA_Above1_ShouldFail()
    {
        var request = new CalculationRequest
        {
            ProbabilityA = 1.1m,
            ProbabilityB = 0.5m,
            CalculationType = CalculationType.Either
        };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.ProbabilityA);
    }

    [Fact]
    public void Validate_NegativeProbabilityB_ShouldFail()
    {
        var request = new CalculationRequest
        {
            ProbabilityA = 0.5m,
            ProbabilityB = -0.5m,
            CalculationType = CalculationType.CombinedWith
        };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.ProbabilityB);
    }

    [Fact]
    public void Validate_ProbabilityB_Above1_ShouldFail()
    {
        var request = new CalculationRequest
        {
            ProbabilityA = 0.5m,
            ProbabilityB = 2m,
            CalculationType = CalculationType.CombinedWith
        };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.ProbabilityB);
    }

    [Fact]
    public void Validate_ExactZero_ShouldPass()
    {
        var request = new CalculationRequest
        {
            ProbabilityA = 0m,
            ProbabilityB = 0m,
            CalculationType = CalculationType.CombinedWith
        };

        _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_ExactOne_ShouldPass()
    {
        var request = new CalculationRequest
        {
            ProbabilityA = 1m,
            ProbabilityB = 1m,
            CalculationType = CalculationType.Either
        };

        _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }
}
