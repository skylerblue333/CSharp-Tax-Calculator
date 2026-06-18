using System;
using System.Collections.Generic;

namespace TaxCalculator
{
    public enum TaxJurisdiction { US_Federal, US_California, UK, EU_Germany }

    public class TaxEngine
    {
        private static readonly Dictionary<TaxJurisdiction, decimal> Rates = new()
        {
            { TaxJurisdiction.US_Federal,    0.22m },
            { TaxJurisdiction.US_California, 0.093m },
            { TaxJurisdiction.UK,            0.20m },
            { TaxJurisdiction.EU_Germany,    0.19m }
        };

        public decimal CalculateTax(decimal income, TaxJurisdiction jurisdiction)
        {
            if (!Rates.TryGetValue(jurisdiction, out var rate))
                throw new ArgumentException($"Unknown jurisdiction: {jurisdiction}");
            return Math.Round(income * rate, 2);
        }

        public decimal CalculateTotalWithTax(decimal income, TaxJurisdiction jurisdiction)
        {
            return income + CalculateTax(income, jurisdiction);
        }
    }

    class Program
    {
        static void Main()
        {
            var engine = new TaxEngine();
            decimal income = 85000m;

            Console.WriteLine($"=== Tax Calculator ===");
            Console.WriteLine($"Base Income: ${income:N2}\n");

            foreach (TaxJurisdiction j in Enum.GetValues<TaxJurisdiction>())
            {
                decimal tax = engine.CalculateTax(income, j);
                decimal total = engine.CalculateTotalWithTax(income, j);
                Console.WriteLine($"  {j,-20} Tax: ${tax,10:N2}  Total: ${total,10:N2}");
            }
        }
    }
}
