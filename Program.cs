using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/v1/calculate", async (HttpContext context) => {
    using var document = await JsonDocument.ParseAsync(context.Request.Body);
    var root = document.RootElement;
    
    if (!root.TryGetProperty("income", out var incomeElement) || !incomeElement.TryGetDouble(out var income)) {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { error = "Invalid or missing 'income' field" });
        return;
    }

    // Progressive tax bracket calculation
    double tax = 0;
    if (income > 100000) {
        tax += (income - 100000) * 0.30;
        income = 100000;
    }
    if (income > 50000) {
        tax += (income - 50000) * 0.20;
        income = 50000;
    }
    tax += income * 0.10;

    await context.Response.WriteAsJsonAsync(new { 
        tax_owed = tax,
        effective_rate = tax / (root.GetProperty("income").GetDouble() == 0 ? 1 : root.GetProperty("income").GetDouble())
    });
});

app.MapGet("/health", () => new { status = "healthy", version = "3.0.0" });

app.Run("http://0.0.0.0:8080");
