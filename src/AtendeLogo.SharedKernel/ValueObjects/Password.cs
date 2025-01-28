using AtendeLogo.Shared.Contantes;

namespace AtendeLogo.Shared.ValueObjects;

public sealed record Password
{
    public string Value { get; }
    public PasswordStrength Strength { get; }

    private Password(string value, string salt)
    {
        Strength = PasswordHelper.CalculateStrength(value);
        Value = PasswordHelper.HashPassword(value, salt);
    }

    public bool Equals(Password? other)
        => other is not null && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value;

    public static Result<Password> Create(string value, string salt)
    {
        if (string.IsNullOrWhiteSpace(salt))
        {
            return Result.ValidationFailure<Password>(
                "Password.SaltEmpty",
                "Salt cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.ValidationFailure<Password>(
                "Password.Empty",
                "Password cannot be empty.");
        }

        if (value.Length < ValidationConstants.PasswordMinLength)
        {
            return Result.ValidationFailure<Password>(
                "Password.TooShort",
                "Password must be at least 6 characters long.");
        }

        if (value.Length > ValidationConstants.PasswordMaxLength)
        {
            return Result.ValidationFailure<Password>(
                "Password.TooLong",
                "Password cannot be longer than 100 characters.");
        }

        var password = new Password(value, salt);
        return Result.Success(password);
    }
}
