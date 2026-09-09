using FinPilot.Domain.Onboarding;
using FinPilot.Domain.Onboarding.ValueObjects;
namespace FinPilot.UnitTests.Onboarding;

public class OnboardingTests
{
    [Fact]
    public void Onboarding_Should_Start_With_Started_Status()
    {
        var onboarding = CreateOnboarding();

        Assert.Equal(OnboardingStatus.Started, onboarding.Status);
    }

    [Fact]
    public void Onboarding_Should_Move_To_IdentityVerified()
    {
        var onboarding = CreateOnboarding();

        onboarding.VerifyIdentity();

        Assert.Equal(OnboardingStatus.IdentityVerified, onboarding.Status);
    }

    [Fact]
    public void Onboarding_Should_Complete_After_IdentityVerification()
    {
        var onboarding = CreateOnboarding();

        onboarding.VerifyIdentity();
        onboarding.Complete();

        Assert.Equal(OnboardingStatus.Completed, onboarding.Status);
    }

    [Fact]
    public void Onboarding_Should_Not_Complete_Before_IdentityVerification()
    {
        var onboarding = CreateOnboarding();

        Assert.Throws<InvalidOperationException>(
            () => onboarding.Complete());
    }

    [Fact]
    public void Completed_Onboarding_Should_Not_Be_Completed_Again()
    {
        var onboarding = CreateOnboarding();

        onboarding.VerifyIdentity();
        onboarding.Complete();

        Assert.Throws<InvalidOperationException>(
            () => onboarding.Complete());
    }

    private static FinPilot.Domain.Onboarding.Onboarding CreateOnboarding()
    {
        return FinPilot.Domain.Onboarding.Onboarding.Start(
            IdentityNumber.Create("12345678901"),
            Email.Create("test@example.com"),
            PhoneNumber.Create("+905551234567"));
    }

    [Fact]
    public void Onboarding_Should_Throw_When_IdentityNumber_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
            FinPilot.Domain.Onboarding.Onboarding.Start(
                null!,
                Email.Create("test@example.com"),
                PhoneNumber.Create("+905551234567")));
    }

    [Fact]
    public void Onboarding_Should_Throw_When_Email_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
            FinPilot.Domain.Onboarding.Onboarding.Start(
                IdentityNumber.Create("12345678901"),
                null!,
                PhoneNumber.Create("+905551234567")));
    }

    [Fact]
    public void Onboarding_Should_Throw_When_PhoneNumber_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
            FinPilot.Domain.Onboarding.Onboarding.Start(
                IdentityNumber.Create("12345678901"),
                Email.Create("test@example.com"),
                null!));
    }
}