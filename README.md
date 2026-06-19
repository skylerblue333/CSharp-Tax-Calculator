# CSharp-Tax-Calculator

![CI](https://github.com/skylerblue333/CSharp-Tax-Calculator/workflows/CI/badge.svg)

High-performance .NET 8 Minimal API for calculating regional tax rates.

## Features
- .NET 8 Minimal APIs for maximum throughput
- Pattern matching for fast region routing
- Dockerized deployment

## Quick Start
```bash
dotnet run
curl -X POST http://localhost:8080/calculate-tax -H "Content-Type: application/json" -d '{"amount": 100, "region": "US-CA"}'
```
