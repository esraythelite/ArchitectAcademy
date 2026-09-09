using FinPilot.Domain.Onboarding.ValueObjects;
namespace FinPilot.UnitTests.Onboarding;
public class PhoneNumberTests
{
    [Fact]
    public void PhoneNumber_Should_Be_Created_When_Valid()
    {
        var phoneNumber = PhoneNumber.Create("+905551234567");

        Assert.Equal("+905551234567", phoneNumber.Value);
    }

    [Fact]
    public void PhoneNumber_Should_Throw_When_Empty()
    {
        Assert.Throws<ArgumentException>(
            () => PhoneNumber.Create(""));
    }

    [Fact]
    public void PhoneNumber_Should_Throw_When_Plus_Is_Missing()
    {
        Assert.Throws<ArgumentException>(
            () => PhoneNumber.Create("905551234567"));
    }

    [Fact]
    public void PhoneNumber_Should_Throw_When_Contains_NonNumeric_Characters()
    {
        Assert.Throws<ArgumentException>(
            () => PhoneNumber.Create("+90555ABC4567"));
    }

    [Fact]
    public void PhoneNumbers_With_Same_Value_Should_Be_Equal()
    {
        var first = PhoneNumber.Create("+905551234567");
        var second = PhoneNumber.Create("+905551234567");

        Assert.Equal(first, second);
    }
}
