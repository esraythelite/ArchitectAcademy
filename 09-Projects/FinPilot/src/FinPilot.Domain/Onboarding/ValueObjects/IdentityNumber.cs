namespace FinPilot.Domain.Onboarding.ValueObjects;
public sealed record IdentityNumber
{
    public string Value { get; }

    private IdentityNumber(string value)
    {
        Value = value;
    }

    public static IdentityNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Identity number cannot be empty.",
                nameof(value));

        value = value.Trim();

        if (value.Length != 11)
            throw new ArgumentException(
                "Identity number must contain 11 characters.",
                nameof(value));

        if (!value.All(char.IsDigit))
            throw new ArgumentException(
                "Identity number must contain only digits.",
                nameof(value));

        //Dışarıdan yapılamıyor. Çünkü invalid bir IdentityNumber instance'ının oluşmasını engelliyoruz.
        return new IdentityNumber(value);
    }
}