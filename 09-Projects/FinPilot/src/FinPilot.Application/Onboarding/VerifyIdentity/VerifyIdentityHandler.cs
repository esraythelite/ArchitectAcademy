using FinPilot.Application.Abstractions.Persistence;
using FinPilot.Application.Onboarding.Persistence;

namespace FinPilot.Application.Onboarding.VerifyIdentity;

public sealed class VerifyIdentityHandler
{
    private readonly IOnboardingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyIdentityHandler(IOnboardingRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VerifyIdentityResult?> HandleAsync(VerifyIdentityCommand command, CancellationToken cancellationToken = default)
    {
        var onboarding = await _repository.GetByIdAsync(command.OnboardingId, cancellationToken);

        if (onboarding == null)
        {
            return null;
        }

        onboarding.VerifyIdentity();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VerifyIdentityResult(onboarding.Id, onboarding.Status.ToString());
    }
}
