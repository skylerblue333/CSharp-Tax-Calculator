# Changelog

## 0.1.0 - 2026-08-24

- replace hardcoded pseudo-tax behavior with caller-supplied progressive rate schedules
- add bounded amount/bracket/rate validation and deterministic decimal rounding
- add native xUnit coverage and remove unrelated Python test/dependency scaffolding
- add health/readiness endpoints, request IDs, and basic security headers
- add Release build, test, vulnerable dependency, container, non-root, and runtime CI gates
- document that the service is jurisdiction-agnostic and does not provide tax advice or current tax-law data
