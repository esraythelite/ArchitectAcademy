using FinPilot.Domain.Onboarding.ValueObjects;
using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;
using FinPilot.Application.Abstractions.Persistence;
using FinPilot.Application.Onboarding.Persistence;
using System.Threading.Tasks;

namespace FinPilot.Application.Onboarding.StartOnboarding;

public sealed class StartOnboardingHandler
{
    private readonly IOnboardingRepository _onboardingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartOnboardingHandler(IOnboardingRepository onboardingRepository, IUnitOfWork unitOfWork)
    {
        _onboardingRepository = onboardingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<StartOnboardingResult> HandleAsync(StartOnboardingCommand command, CancellationToken cancellationToken = default)
    {
        IdentityNumber identityNumber = IdentityNumber.Create(command.IdentityNumber);
        Email email = Email.Create(command.Email);
        PhoneNumber phoneNumber = PhoneNumber.Create(command.PhoneNumber);
        var onboarding = OnboardingAggregate.Start(
                                                        identityNumber,
                                                        email,
                                                        phoneNumber
                                                        );

        await _onboardingRepository.AddAsync(onboarding, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return new StartOnboardingResult(onboarding.Id, onboarding.Status.ToString());
    }
}

