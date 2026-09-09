using FinPilot.Domain.Onboarding.ValueObjects;
namespace FinPilot.UnitTests.Onboarding;


public class EmailTests
{
    [Fact]
    public void Email_Should_Be_Created_When_Valid()
    {
        var email = Email.Create("test@example.com");

        Assert.Equal("test@example.com", email.Value);
    }

    [Fact]
    public void Email_Should_Be_Normalized()
    {
        var email = Email.Create("  TEST@EXAMPLE.COM  ");

        Assert.Equal("test@example.com", email.Value);
    }

    [Fact]
    public void Email_Should_Throw_When_Empty()
    {
        Assert.Throws<ArgumentException>(() => Email.Create(""));
    }

    [Fact]
    public void Email_Should_Throw_When_Format_Is_Invalid()
    {
        Assert.Throws<ArgumentException>(() => Email.Create("invalid-email"));
    }

    [Fact]
    public void Emails_With_Same_Normalized_Value_Should_Be_Equal()
    {
        var first = Email.Create("TEST@example.com");
        var second = Email.Create("test@example.com");

        Assert.Equal(first, second);
    }
}