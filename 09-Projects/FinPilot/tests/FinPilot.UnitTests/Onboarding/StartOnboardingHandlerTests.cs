using FinPilot.Application.Onboarding.StartOnboarding;
namespace FinPilot.UnitTests.Onboarding;

public class StartOnboardingHandlerTests
{
    [Fact]
    public void Handle_ValidCommand_ReturnsExpectedResult()
    {
        // Arrange
        var command = new StartOnboardingCommand("11111111111", "test@example.com", "+1234567890");
        var handler = new StartOnboardingHandler();

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Started", result.Status);
    }

        [Fact]
    public void Handle_InvalidIdentityNumber_ThrowsArgumentException()
    {
        // Arrange
        var command = new StartOnboardingCommand("", "test@example.com", "+1234567890");
        var handler = new StartOnboardingHandler();

        // Assert
        Assert.Throws<ArgumentException>(() => handler.Handle(command));
    }
}
