using FinPilot.Application.Abstractions.Persistence;
using FinPilot.Application.Onboarding.Persistence;

namespace FinPilot.Application.Onboarding.CompleteOnboarding;

public sealed class CompleteOnboardingHandler
{
    private readonly IOnboardingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteOnboardingHandler(IOnboardingRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompleteOnboardingResult?> HandleAsync(CompleteOnboardingCommand command, CancellationToken cancellationToken = default)
    {
        var onboarding = await _repository.GetByIdAsync(command.OnboardingId, cancellationToken);

        if (onboarding == null)
        {
            return null;
        }

        onboarding.Complete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CompleteOnboardingResult(onboarding.Id, onboarding.Status.ToString());
    }
}
