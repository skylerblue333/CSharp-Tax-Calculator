# CSharp-Tax-Calculator

![CI](https://github.com/skylerblue333/CSharp-Tax-Calculator/workflows/CI/badge.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4.svg)

A high-performance progressive tax calculation engine built with .NET 8 Minimal APIs.

## System Architecture

```mermaid
graph LR
    Client-->|JSON/HTTP|API[.NET 8 Minimal API]
    API-->Engine[Progressive Tax Engine]
    Engine-->Rules[Bracket Rule Definitions]
```

## Elite Features
- **.NET 8 Minimal APIs**: Ultra-low overhead HTTP routing.
- **Async I/O**: `System.Text.Json` asynchronous parsing.
- **Progressive Brackets**: Real-world cascading tax logic.
