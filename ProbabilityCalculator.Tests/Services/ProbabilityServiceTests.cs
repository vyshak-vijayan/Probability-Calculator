using FluentAssertions;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Services;

namespace ProbabilityCalculator.Tests.Services;

public class ProbabilityServiceTests
{
    private readonly ProbabilityService _sut = new();

    [Fact]
    public void CombinedWith_StandardValues_ReturnsProduct()
    {
        var request = MakeRequest(0.5m, 0.5m, CalculationType.CombinedWith);

        var result = _sut.Calculate(request);

        result.Result.Should().Be(0.25m);
    }

    [Fact]
    public void CombinedWith_ZeroProbability_ReturnsZero()
    {
        var result = _sut.Calculate(MakeRequest(0m, 0.5m, CalculationType.CombinedWith));
        result.Result.Should().Be(0m);
    }

    [Fact]
    public void CombinedWith_OneProbability_ReturnsOther()
    {
        var result = _sut.Calculate(MakeRequest(1m, 0.7m, CalculationType.CombinedWith));
        result.Result.Should().Be(0.7m);
    }

    [Fact]
    public void CombinedWith_BothOne_ReturnsOne()
    {
        var result = _sut.Calculate(MakeRequest(1m, 1m, CalculationType.CombinedWith));
        result.Result.Should().Be(1m);
    }

    [Fact]
    public void CombinedWith_BothZero_ReturnsZero()
    {
        var result = _sut.Calculate(MakeRequest(0m, 0m, CalculationType.CombinedWith));
        result.Result.Should().Be(0m);
    }

    [Fact]
    public void Either_StandardValues_ReturnsInclusiveOr()
    {
        var result = _sut.Calculate(MakeRequest(0.5m, 0.5m, CalculationType.Either));
        result.Result.Should().Be(0.75m);
    }

    [Fact]
    public void Either_ZeroProbability_ReturnsOther()
    {
        var result = _sut.Calculate(MakeRequest(0m, 0.5m, CalculationType.Either));
        result.Result.Should().Be(0.5m);
    }

    [Fact]
    public void Either_OneProbability_ReturnsOne()
    {
        var result = _sut.Calculate(MakeRequest(1m, 0.3m, CalculationType.Either));
        result.Result.Should().Be(1m);
    }

    [Fact]
    public void Either_BothZero_ReturnsZero()
    {
        var result = _sut.Calculate(MakeRequest(0m, 0m, CalculationType.Either));
        result.Result.Should().Be(0m);
    }

    [Fact]
    public void Either_BothOne_ReturnsOne()
    {
        var result = _sut.Calculate(MakeRequest(1m, 1m, CalculationType.Either));
        result.Result.Should().Be(1m);
    }

    [Fact]
    public void Calculate_EchoesInputsInResult()
    {
        var request = MakeRequest(0.3m, 0.6m, CalculationType.Either);

        var result = _sut.Calculate(request);

        result.ProbabilityA.Should().Be(0.3m);
        result.ProbabilityB.Should().Be(0.6m);
        result.CalculationType.Should().Be(CalculationType.Either);
    }

    private static CalculationRequest MakeRequest(decimal a, decimal b, CalculationType type) =>
        new() { ProbabilityA = a, ProbabilityB = b, CalculationType = type };
}
