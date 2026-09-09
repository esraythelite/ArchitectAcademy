namespace FinPilot.Domain.Onboarding.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Phone number cannot be empty.",
                nameof(value));

        value = value.Trim();

        if (!IsValidPhoneNumber(value))
            throw new ArgumentException(
                "Invalid phone number format.",
                nameof(value));

        return new PhoneNumber(value);
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (!phoneNumber.StartsWith('+'))
            return false;

        var digits = phoneNumber[1..];

        return digits.Length >= 8 &&
               digits.Length <= 15 &&
               digits.All(char.IsDigit);
    }
}
