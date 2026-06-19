using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.MapPost("/calculate-tax", (TaxRequest req) => {
    if (req.Amount < 0) return Results.BadRequest("Amount cannot be negative");
    
    decimal rate = req.Region switch {
        "US-CA" => 0.0725m,
        "US-NY" => 0.08875m,
        "UK" => 0.20m,
        _ => 0.0m
    };
    
    decimal tax = req.Amount * rate;
    return Results.Ok(new TaxResponse(req.Amount, tax, req.Amount + tax, rate));
});

app.Run("http://0.0.0.0:8080");

public record TaxRequest(decimal Amount, string Region);
public record TaxResponse(decimal Subtotal, decimal Tax, decimal Total, decimal Rate);
