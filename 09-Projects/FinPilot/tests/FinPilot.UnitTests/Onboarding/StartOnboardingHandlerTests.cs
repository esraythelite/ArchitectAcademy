using FinPilot.Application.Onboarding.StartOnboarding;
using FinPilot.Application.Onboarding.Persistence;
using FinPilot.Application.Abstractions.Persistence;
using OnboardingAggregate = FinPilot.Domain.Onboarding.Onboarding;
using System.Threading.Tasks;
public class StartOnboardingHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsExpectedResult()
    {
        // Arrange

        var fakeRepository = new FakeOnboardingRepository();
        var fakeUnitOfWork = new FakeUnitOfWork();

        var command = new StartOnboardingCommand("11111111111", "test@example.com", "+1234567890");
        var handler = new StartOnboardingHandler(fakeRepository, fakeUnitOfWork);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Started", result.Status);

        Assert.NotNull(fakeRepository.AddedOnboarding);
        Assert.True(fakeUnitOfWork.SaveChangesCalled);
    }

    [Fact]
    public async Task Handle_InvalidIdentityNumber_ThrowsArgumentException()
    {
        var fakeRepository = new FakeOnboardingRepository();
        var fakeUnitOfWork = new FakeUnitOfWork();
        // Arrange
        var command = new StartOnboardingCommand("", "test@example.com", "+1234567890");
        var handler = new StartOnboardingHandler(fakeRepository, fakeUnitOfWork);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(command));
        Assert.Null(fakeRepository.AddedOnboarding);
        Assert.False(fakeUnitOfWork.SaveChangesCalled);
    }
}

public sealed class FakeOnboardingRepository : IOnboardingRepository
{
    public OnboardingAggregate? AddedOnboarding { get; private set; }

    public Task AddAsync(OnboardingAggregate onboarding, CancellationToken cancellationToken = default)
    {
        AddedOnboarding = onboarding;
        return Task.CompletedTask;
    }

    public Task<OnboardingAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<OnboardingAggregate?>(null);
    }
}

public sealed class FakeUnitOfWork : IUnitOfWork
{
    public bool SaveChangesCalled { get; private set; } 
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCalled = true;
        return Task.FromResult(1);
    }
}
