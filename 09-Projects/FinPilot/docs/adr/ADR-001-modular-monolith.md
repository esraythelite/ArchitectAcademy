# ADR-001: Start with a Modular Monolith

## Status

Accepted

## Context

FinPilot is a new system and its domain boundaries are still evolving.

Starting directly with microservices would introduce distributed-system
complexity before the boundaries and scaling requirements are understood.

## Decision

FinPilot will initially be implemented as a modular monolith.

Modules will have explicit boundaries and will avoid direct coupling where
possible so that selected modules can later be extracted into independent
services.

## Consequences

### Positive

- Faster initial development
- Simpler local development and deployment
- Easier transactions
- Domain boundaries can evolve
- Lower operational complexity

### Negative

- Modules share a deployment unit
- Poor discipline could create unwanted coupling
- Independent scaling is not initially possible

## Revisit When

- A module requires independent scaling
- Deployment cadence differs significantly
- Team ownership requires separation
- Availability requirements differ