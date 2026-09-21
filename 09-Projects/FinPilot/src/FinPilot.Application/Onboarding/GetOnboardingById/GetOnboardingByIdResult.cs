namespace FinPilot.Application.Onboarding.GetOnboardingById;

public sealed record GetOnboardingByIdResult(
    Guid Id,
    string IdentityNumber,
    string Email,
    string PhoneNumber,
    string Status,
    DateTime CreatedAtUtc);