using FluentValidation;
using ProbabilityCalculator.Api.Models;

namespace ProbabilityCalculator.Api.Validators;

public class CalculationRequestValidator : AbstractValidator<CalculationRequest>
{
    public CalculationRequestValidator()
    {
        RuleFor(x => x.ProbabilityA)
            .InclusiveBetween(0m, 1m)
            .WithMessage("Probability A must be between 0 and 1.");

        RuleFor(x => x.ProbabilityB)
            .InclusiveBetween(0m, 1m)
            .WithMessage("Probability B must be between 0 and 1.");

        RuleFor(x => x.CalculationType)
            .IsInEnum()
            .WithMessage("Calculation type must be either 'CombinedWith' or 'Either'.");
    }
}
