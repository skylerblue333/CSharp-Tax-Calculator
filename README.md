# Sky Tax Calculator

Sky Tax Calculator is a small .NET 8 service and library for applying **caller-supplied progressive rate schedules** to a numeric amount. It is jurisdiction-agnostic by design: the repository does not embed or claim to maintain current federal, state, local, or international tax law.

**Status: engineering beta.** This project is suitable for deterministic calculation experiments and integration testing, not for filing taxes or making legal/financial decisions without an independently validated rate schedule and professional review.

## API

`GET /healthz` provides liveness. `GET /readyz` identifies the calculator mode. `POST /v1/calculate` accepts an amount and 1–32 progressive brackets. Rates are decimal fractions from `0` to `1`; only the final bracket may have no upper bound.

Example request:

```json
{
  "amount": 120000,
  "scheduleName": "example-only",
  "brackets": [
    {"upTo": 50000, "rate": 0.10},
    {"upTo": 100000, "rate": 0.20},
    {"upTo": null, "rate": 0.30}
  ]
}
```

For this example schedule the service returns a tax value of `21000.00` and an effective rate of `0.175`. Those numbers describe only the supplied example; they are not a statement of real-world tax liability.

## Validation

The calculator rejects negative amounts, amounts above one trillion, missing/excessive brackets, rates outside `0..1`, non-increasing upper bounds, non-final unbounded brackets, and schedules that do not cover the requested amount. Monetary tax output is rounded to two decimal places using `MidpointRounding.AwayFromZero`.

## Local verification

```bash
dotnet restore tests/TaxCalculator.Tests.csproj
dotnet build CSharp-Tax-Calculator.csproj -c Release --no-restore
dotnet test tests/TaxCalculator.Tests.csproj -c Release --no-restore
dotnet list CSharp-Tax-Calculator.csproj package --vulnerable --include-transitive
```

Container verification:

```bash
docker build -t sky-tax .
docker run --rm -p 8080:8080 sky-tax
curl http://127.0.0.1:8080/healthz
```

The runtime image executes as the built-in non-root `app` user. CI performs restore, Release build, xUnit tests, vulnerable dependency inspection, container build, non-root verification, and a live health check.

## SKYCOIN4444 integration

The calculator can be consumed through a stable HTTP adapter from finance or marketplace modules when a validated rate schedule is supplied by a separate policy/configuration source. Keep tax policy data outside this repository so code and jurisdiction-specific rules have independent ownership and audit trails.

## Limits

This repository does not provide tax advice, jurisdiction detection, deductions, credits, filing status logic, currency conversion, payroll withholding, tax-form generation, policy updates, authentication, persistent audit history, or production deployment evidence.
