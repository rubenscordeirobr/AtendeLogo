﻿using AtendeLogo.Shared.Contantes;

namespace AtendeLogo.Shared.ValueObjects;

public sealed record Password : ValueObjectBase
{
    public string HashValue { get; private set; }
    public PasswordStrength Strength { get; private set; }

    public bool IsEmpty 
        => Strength == PasswordStrength.Empty;

    private Password(string value, string salt)
    {
        Strength = PasswordHelper.CalculateStrength(value);
        HashValue = PasswordHelper.HashPassword(value, salt);
    }

    public Password(string hashValue, PasswordStrength strength)
    {
        if (strength != PasswordStrength.Empty)
        {
            Guard.Sha256(hashValue);
        }
        HashValue = hashValue;
        Strength = strength;
    }

    public bool Equals(Password? other)
        => other is not null && HashValue == other.HashValue;

    public override int GetHashCode()
        => HashValue.GetHashCode();

    public override string ToString()
        => HashValue;

    public static Password Empty
        => new(string.Empty, PasswordStrength.Empty);

    public static Password RandomPassword()
        => new(PasswordHelper.GenerateRandomPassword(), PasswordStrength.Strong);

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
