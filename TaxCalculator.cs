namespace SkyTax;

public sealed record TaxBracket(decimal? UpTo, decimal Rate);
public sealed record TaxResult(decimal Amount, decimal Tax, decimal EffectiveRate);

public static class ProgressiveTaxCalculator
{
    public static TaxResult Calculate(decimal amount, IReadOnlyList<TaxBracket> brackets)
    {
        if (amount < 0 || amount > 1_000_000_000_000m)
            throw new ArgumentOutOfRangeException(nameof(amount), "amount must be between 0 and 1 trillion");
        if (brackets.Count is < 1 or > 32)
            throw new ArgumentException("between 1 and 32 brackets are required", nameof(brackets));

        decimal previousUpper = 0m;
        decimal tax = 0m;

        for (var index = 0; index < brackets.Count; index++)
        {
            var bracket = brackets[index];
            if (bracket.Rate is < 0m or > 1m)
                throw new ArgumentException("bracket rates must be between 0 and 1", nameof(brackets));

            if (bracket.UpTo is null)
            {
                if (index != brackets.Count - 1)
                    throw new ArgumentException("only the final bracket may omit an upper bound", nameof(brackets));
                if (amount > previousUpper)
                    tax += (amount - previousUpper) * bracket.Rate;
                previousUpper = amount;
                break;
            }

            var upper = bracket.UpTo.Value;
            if (upper <= previousUpper)
                throw new ArgumentException("bracket upper bounds must be strictly increasing", nameof(brackets));

            if (amount > previousUpper)
            {
                var taxable = Math.Min(amount, upper) - previousUpper;
                tax += taxable * bracket.Rate;
            }

            previousUpper = upper;
            if (amount <= upper)
                break;
        }

        if (amount > previousUpper)
            throw new ArgumentException("brackets do not cover the full amount", nameof(brackets));

        tax = decimal.Round(tax, 2, MidpointRounding.AwayFromZero);
        var effectiveRate = amount == 0m ? 0m : decimal.Round(tax / amount, 6, MidpointRounding.AwayFromZero);
        return new TaxResult(amount, tax, effectiveRate);
    }
}
