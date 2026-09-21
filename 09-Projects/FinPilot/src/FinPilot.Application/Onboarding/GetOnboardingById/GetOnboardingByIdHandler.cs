using FinPilot.Application.Onboarding.Persistence;

namespace FinPilot.Application.Onboarding.GetOnboardingById;

public sealed class GetOnboardingByIdHandler
{
    private readonly IOnboardingRepository _repository;

    public GetOnboardingByIdHandler(IOnboardingRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetOnboardingByIdResult?> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var onboarding = await _repository.GetByIdAsync(id, cancellationToken);

        if (onboarding == null)
        {
            return null;
        }

        return new GetOnboardingByIdResult(
            onboarding.Id,
            onboarding.IdentityNumber.Value,
            onboarding.Email.Value,
            onboarding.PhoneNumber.Value,
            onboarding.Status.ToString(),
            onboarding.CreatedAtUtc);
    }
}