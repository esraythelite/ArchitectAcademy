namespace FinPilot.Api.Onboarding;

public sealed record StartOnboardingRequest(
    string IdentityNumber,
    string Email,
    string PhoneNumber);
