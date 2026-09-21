using FinPilot.Application.Onboarding.Persistence;
using Microsoft.EntityFrameworkCore;

using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;

namespace FinPilot.Infrastructure.Persistence.Repositories;

public sealed class OnboardingRepository : IOnboardingRepository
{
    private readonly FinPilotDbContext _dbContext;

    public OnboardingRepository(FinPilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OnboardingAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Onboardings.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task AddAsync(OnboardingAggregate onboarding, CancellationToken cancellationToken = default)
    {
        await _dbContext.Onboardings.AddAsync(onboarding, cancellationToken);
    }
}
