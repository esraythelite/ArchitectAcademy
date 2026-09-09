namespace FinPilot.Domain.Onboarding.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Email cannot be empty.",
                nameof(value));

        value = value.Trim().ToLowerInvariant();

        if (!IsValidEmail(value))
            throw new ArgumentException(
                "Invalid email format.",
                nameof(value));

        return new Email(value);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

