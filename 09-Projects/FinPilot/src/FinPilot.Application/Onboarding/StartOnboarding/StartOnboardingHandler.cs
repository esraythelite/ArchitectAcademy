using FinPilot.Domain.Onboarding.ValueObjects;
using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;

namespace FinPilot.Application.Onboarding.StartOnboarding;

public class StartOnboardingHandler
{
    public StartOnboardingResult Handle(StartOnboardingCommand command)
    {
        IdentityNumber identityNumber = IdentityNumber.Create(command.IdentityNumber);
        Email email = Email.Create(command.Email);
        PhoneNumber phoneNumber = PhoneNumber.Create(command.PhoneNumber);  
        var onboarding = OnboardingAggregate.Start(
                                                        identityNumber,
                                                        email,
                                                        phoneNumber
                                                        );                                       

        return new StartOnboardingResult(onboarding.Id, onboarding.Status.ToString());
    }
}

