using FinPilot.Application.Onboarding.GetOnboardingById;
using FinPilot.Application.Onboarding.Persistence;
using FinPilot.Domain.Onboarding.ValueObjects;
using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;

namespace FinPilot.UnitTests.Onboarding;

public sealed class GetOnboardingByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenOnboardingExists_ReturnsExpectedResult()
    {
        // Given
        var onboarding = OnboardingAggregate.Start(
            IdentityNumber.Create("11111111111"),
            Email.Create("test@example.com"),
            PhoneNumber.Create("+905551234567"));

        var repository = new FakeOnboardingRepository(onboarding);

        var handler = new GetOnboardingByIdHandler(repository);

        var result = await handler.HandleAsync(onboarding.Id);

        Assert.NotNull(result);

        Assert.Equal(onboarding.Id, result.Id);
        Assert.Equal("11111111111", result.IdentityNumber);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("+905551234567", result.PhoneNumber);
        Assert.Equal("Started", result.Status);
    }

    [Fact]
    public async Task Handle_WhenOnboardingDoesNotExist_ReturnsNull()
    {
        // Given
        var repository = new FakeOnboardingRepository(null);

        var handler = new GetOnboardingByIdHandler(repository);

        var result = await handler.HandleAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    private sealed class FakeOnboardingRepository : IOnboardingRepository
    {
        private readonly OnboardingAggregate? _onboarding;

        public FakeOnboardingRepository(OnboardingAggregate? onboarding)
        {
            _onboarding = onboarding;
        }

        public Task AddAsync(OnboardingAggregate onboarding, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<OnboardingAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_onboarding?.Id == id ? _onboarding : null);
        }
    }
}
