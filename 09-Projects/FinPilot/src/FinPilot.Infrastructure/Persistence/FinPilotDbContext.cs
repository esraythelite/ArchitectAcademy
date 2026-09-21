using FinPilot.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;

namespace FinPilot.Infrastructure.Persistence;

public sealed class FinPilotDbContext : DbContext, IUnitOfWork
{
    public FinPilotDbContext(DbContextOptions<FinPilotDbContext> options)
        : base(options)
    {
    }

    public DbSet<OnboardingAggregate> Onboardings => Set<OnboardingAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinPilotDbContext).Assembly);
    }
}