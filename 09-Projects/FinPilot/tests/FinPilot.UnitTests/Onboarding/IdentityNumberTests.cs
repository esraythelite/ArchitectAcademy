using FinPilot.Domain.Onboarding.ValueObjects;

namespace FinPilot.UnitTests.Onboarding;

public class IdentityNumberTests
{
    [Fact]
    public void IdentityNumbers_With_Same_Value_Should_Be_Equal()
    {
        var first = IdentityNumber.Create("12345678901");
        var second = IdentityNumber.Create("12345678901");

        Assert.Equal(first, second);
        Assert.True(first == second);
    }

    [Fact]
    public void IdentityNumbers_With_Different_Values_Should_Not_Be_Equal()
    {
        var first = IdentityNumber.Create("12345678901");
        var second = IdentityNumber.Create("09876543210");

        Assert.NotEqual(first, second);
        Assert.False(first == second);
    }
}