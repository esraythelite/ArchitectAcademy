# FinPilot System Context

```mermaid
flowchart LR

    User[Investor]

    FinPilot[FinPilot Platform]

    IdentityProvider[Identity Provider]

    MarketProvider[Market Data / Brokerage Provider]

    AIProvider[AI Provider]

    User --> FinPilot

    FinPilot --> IdentityProvider
    FinPilot --> MarketProvider
    FinPilot --> AIProvider