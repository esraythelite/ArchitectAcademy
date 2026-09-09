# ADR-002: Use PostgreSQL as the Primary Database

## Status

Accepted

## Context

FinPilot requires a relational database for transactional financial data.

The system will contain strongly related entities such as:

- Customers
- Accounts
- Portfolios
- Orders
- Transactions
- Onboarding processes

Data consistency is an important architectural requirement.

## Decision

PostgreSQL will be used as the primary transactional database.

## Alternatives Considered

### SQL Server

A strong relational database and suitable for financial systems.

Not selected because PostgreSQL provides everything required for this project
while also being easy to run locally and inside containers.

### MongoDB

Provides flexible document storage.

Not selected as the primary transactional database because the FinPilot domain
requires strong relational consistency.

## Consequences

### Positive

- ACID transactions
- Strong relational model
- Mature ecosystem
- Excellent .NET support
- Easy Docker integration

### Negative

- Horizontal scaling requires additional architectural considerations
- Schema changes require migration management

## Revisit When

- The system reaches significant scale
- A module develops substantially different persistence requirements
- Analytical workloads require a specialized database