using FinPilot.Application.Abstractions.Persistence;
using FinPilot.Application.Onboarding.Persistence;
using FinPilot.Application.Onboarding.VerifyIdentity;
using FinPilot.Domain.Onboarding.ValueObjects;
using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;

namespace FinPilot.UnitTests.Onboarding;

public sealed class VerifyIdentityHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenOnboardingExists_VerifiesIdentity()
    {
        var onboarding = OnboardingAggregate.Start(
            IdentityNumber.Create("11111111111"),
            Email.Create("verify@example.com"),
            PhoneNumber.Create("+905551234567"));

        var repository = new FakeOnboardingRepository(onboarding);
        var unitOfWork = new FakeUnitOfWork();

        var handler = new VerifyIdentityHandler(
            repository,
            unitOfWork);

        var command = new VerifyIdentityCommand(onboarding.Id);

        var result = await handler.HandleAsync(command);

        Assert.NotNull(result);
        Assert.Equal("IdentityVerified", result.Status);
        Assert.Equal(
            Domain.Onboarding.OnboardingStatus.IdentityVerified,
            onboarding.Status);

        Assert.True(unitOfWork.SaveChangesCalled);
    }

    [Fact]
    public async Task HandleAsync_WhenOnboardingDoesNotExist_ReturnsNull()
    {
        var repository = new FakeOnboardingRepository(null);
        var unitOfWork = new FakeUnitOfWork();

        var handler = new VerifyIdentityHandler(
            repository,
            unitOfWork);

        var result = await handler.HandleAsync(
            new VerifyIdentityCommand(Guid.NewGuid()));

        Assert.Null(result);
        Assert.False(unitOfWork.SaveChangesCalled);
    }

    private sealed class FakeOnboardingRepository
        : IOnboardingRepository
    {
        private readonly OnboardingAggregate? _onboarding;

        public FakeOnboardingRepository(
            OnboardingAggregate? onboarding)
        {
            _onboarding = onboarding;
        }

        public Task AddAsync(
            OnboardingAggregate onboarding,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<OnboardingAggregate?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _onboarding?.Id == id
                    ? _onboarding
                    : null);
        }
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public bool SaveChangesCalled { get; private set; }

        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            SaveChangesCalled = true;
            return Task.FromResult(1);
        }
    }
}