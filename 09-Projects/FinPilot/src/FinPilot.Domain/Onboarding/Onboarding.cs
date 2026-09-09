namespace FinPilot.Domain.Onboarding;

using FinPilot.Domain.Onboarding.ValueObjects;

public sealed class Onboarding
{
    public Guid Id { get; private set; }

    public IdentityNumber IdentityNumber { get; private set; }

    public Email Email { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public OnboardingStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private Onboarding(
        Guid id,
        IdentityNumber identityNumber,
        Email email,
        PhoneNumber phoneNumber)
    {
        Id = id;
        IdentityNumber = identityNumber;
        Email = email;
        PhoneNumber = phoneNumber;

        Status = OnboardingStatus.Started;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Onboarding Start(
        IdentityNumber identityNumber,
        Email email,
        PhoneNumber phoneNumber)
    {
        ArgumentNullException.ThrowIfNull(identityNumber);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(phoneNumber);

        return new Onboarding(
            Guid.NewGuid(),
            identityNumber,
            email,
            phoneNumber);
    }

    public void VerifyIdentity()
    {
        if (Status != OnboardingStatus.Started)
            throw new InvalidOperationException(
                "Identity can only be verified when onboarding is in 'Started' status.");

        Status = OnboardingStatus.IdentityVerified;
    }

    public void Complete()
    {
        if (Status != OnboardingStatus.IdentityVerified)
            throw new InvalidOperationException(
                "Onboarding can only be completed when identity is verified.");

        Status = OnboardingStatus.Completed;
    }
}