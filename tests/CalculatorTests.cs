using SkyTax;

namespace SkyTax.Tests;

public sealed class CalculatorTests
{
    private static readonly TaxBracket[] ExampleSchedule =
    [
        new(50_000m, 0.10m),
        new(100_000m, 0.20m),
        new(null, 0.30m),
    ];

    [Fact]
    public void CalculatesProgressiveTaxAcrossBrackets()
    {
        var result = ProgressiveTaxCalculator.Calculate(120_000m, ExampleSchedule);

        Assert.Equal(21_000m, result.Tax);
        Assert.Equal(0.175m, result.EffectiveRate);
    }

    [Fact]
    public void ZeroAmountHasZeroTaxAndRate()
    {
        var result = ProgressiveTaxCalculator.Calculate(0m, ExampleSchedule);

        Assert.Equal(0m, result.Tax);
        Assert.Equal(0m, result.EffectiveRate);
    }

    [Fact]
    public void RejectsNegativeAmount() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => ProgressiveTaxCalculator.Calculate(-1m, ExampleSchedule));

    [Fact]
    public void RejectsNonIncreasingBrackets()
    {
        TaxBracket[] invalid = [new(100m, 0.1m), new(100m, 0.2m), new(null, 0.3m)];
        Assert.Throws<ArgumentException>(() => ProgressiveTaxCalculator.Calculate(200m, invalid));
    }

    [Fact]
    public void RejectsScheduleThatDoesNotCoverAmount()
    {
        TaxBracket[] incomplete = [new(100m, 0.1m)];
        Assert.Throws<ArgumentException>(() => ProgressiveTaxCalculator.Calculate(200m, incomplete));
    }

    [Fact]
    public void RejectsRateAboveOne()
    {
        TaxBracket[] invalid = [new(null, 1.01m)];
        Assert.Throws<ArgumentException>(() => ProgressiveTaxCalculator.Calculate(100m, invalid));
    }
}
