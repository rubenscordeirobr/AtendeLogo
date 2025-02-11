using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.Common;

public static class Guard
{
    public static void NotNull<T>(
        [NotNull] T value,
        [CallerArgumentExpression("value")] string? paramName = "")
    {
        if (value is null)
            throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
    }

    public static void NotNullOrWhiteSpace(
        [NotNull] string? value,
        [CallerArgumentExpression("value")] string? paramName = "")
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null, empty, or whitespace.", paramName);
    }

    public static void FullPhoneNumber(
        [NotNull] string? fullNumber,
        [CallerArgumentExpression("fullNumber")] string? paramName = "")
    {
        if (fullNumber is null)
            throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");

        if (!PhoneNumberUtils.IsFullPhoneNumberValid(fullNumber))
            throw new ArgumentException($"{paramName} is not a valid phone number.", paramName);
    }

    public static void Positive(
        int value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be greater than zero.");
    }

    public static void Sha256(
        [NotNull] string? value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value is null)
            throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");

        if (!ValidationUtils.IsSha256(value))
            throw new ArgumentException($"{paramName} must be a SHA-256 hash value.", paramName);
    }

    public static void NotEmpty<T>(T value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value is null)
            throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");

        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new ArgumentException($"{paramName} cannot be empty.", paramName);
    }
     
    public static void MustBeEmpty<T>(T value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value is null)
            return;

        if (!EqualityComparer<T>.Default.Equals(value, default))
            throw new ArgumentException($"{paramName} must be empty.", paramName);
    }
}
