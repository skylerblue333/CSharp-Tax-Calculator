using SkyTax;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    context.Response.Headers["X-Request-Id"] = context.Request.Headers["X-Request-Id"].FirstOrDefault() ?? Guid.NewGuid().ToString();
    await next();
});

app.MapGet("/healthz", () => Results.Ok(new { status = "ok", service = "sky-tax" }));
app.MapGet("/readyz", () => Results.Ok(new { status = "ready", calculator = "progressive-rate" }));

app.MapPost("/v1/calculate", (CalculationRequest request) =>
{
    try
    {
        var brackets = request.Brackets
            .Select(bracket => new TaxBracket(bracket.UpTo, bracket.Rate))
            .ToArray();
        var result = ProgressiveTaxCalculator.Calculate(request.Amount, brackets);
        return Results.Ok(new CalculationResponse(
            result.Amount,
            result.Tax,
            result.EffectiveRate,
            request.ScheduleName ?? "caller-supplied",
            brackets.Length));
    }
    catch (ArgumentException error)
    {
        return Results.BadRequest(new { error = error.Message });
    }
});

app.Run();

public sealed record BracketRequest(decimal? UpTo, decimal Rate);
public sealed record CalculationRequest(decimal Amount, IReadOnlyList<BracketRequest> Brackets, string? ScheduleName);
public sealed record CalculationResponse(decimal Amount, decimal Tax, decimal EffectiveRate, string ScheduleName, int BracketCount);

public partial class Program { }
