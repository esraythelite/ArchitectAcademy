using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;

namespace FinPilot.Application.Onboarding.Persistence;

public interface IOnboardingRepository
{
    Task<OnboardingAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(OnboardingAggregate onboarding, CancellationToken cancellationToken = default);
}
