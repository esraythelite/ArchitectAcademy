using FinPilot.Application.Onboarding.CompleteOnboarding;
using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;
using FinPilot.Application.Abstractions.Persistence;
using FinPilot.Application.Onboarding.Persistence;
using FinPilot.Domain.Onboarding.ValueObjects;
namespace FinPilot.UnitTests.Onboarding;

public sealed class CompleteOnboardingHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenIdentityIsVerified_CompletesOnboarding()
    {
        var onboarding = OnboardingAggregate.Start(
       IdentityNumber.Create("11111111111"),
       Email.Create("complete@example.com"),
       PhoneNumber.Create("+905551234567"));

        onboarding.VerifyIdentity();

        var repository = new FakeOnboardingRepository(onboarding);
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CompleteOnboardingHandler(
            repository,
            unitOfWork);

        var result = await handler.HandleAsync(
            new CompleteOnboardingCommand(onboarding.Id));

        Assert.NotNull(result);
        Assert.Equal("Completed", result.Status);
        Assert.Equal(
             Domain.Onboarding.OnboardingStatus.Completed,
            onboarding.Status);

        Assert.True(unitOfWork.SaveChangesCalled);
    }
    [Fact]
    public async Task HandleAsync_WhenOnboardingDoesNotExist_ReturnsNull()
    {
        var repository = new FakeOnboardingRepository(null);
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CompleteOnboardingHandler(
            repository,
            unitOfWork);

        var result = await handler.HandleAsync(
            new CompleteOnboardingCommand(Guid.NewGuid()));

        Assert.Null(result);
        Assert.False(unitOfWork.SaveChangesCalled);
    }
    [Fact]
    public async Task HandleAsync_WhenIdentityIsNotVerified_ThrowsInvalidOperationException()
    {
        var onboarding = OnboardingAggregate.Start(
            IdentityNumber.Create("11111111111"),
            Email.Create("complete@example.com"),
            PhoneNumber.Create("+905551234567"));

        var repository = new FakeOnboardingRepository(onboarding);
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CompleteOnboardingHandler(
            repository,
            unitOfWork);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(
                new CompleteOnboardingCommand(onboarding.Id)));

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