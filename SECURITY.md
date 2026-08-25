# Security

Sky Tax Calculator is an engineering-beta calculation service. Report suspected vulnerabilities privately to the repository owner rather than publishing exploit details in an issue.

## Current controls

- bounded amount and bracket counts
- bracket ordering/rate validation
- no embedded tax-policy secrets or jurisdiction credentials
- request IDs and basic security response headers
- vulnerable dependency inspection in CI
- non-root container execution

## Not implemented

Authentication, authorization, TLS termination, rate limiting, persistent audit logs, jurisdiction-policy signing, secrets management, and production deployment hardening are outside the current repository scope. Do not expose the service directly to untrusted public traffic without those controls.
