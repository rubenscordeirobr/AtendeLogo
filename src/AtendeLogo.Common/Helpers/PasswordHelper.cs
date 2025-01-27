using AtendeLogo.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace AtendeLogo.Domain.Helpers;
public static class PasswordHelper
{
    private static bool ContainsUpperCase(string input)
        => input.Any(char.IsUpper);
    private static bool ContainsNumber(string input)
        => input.Any(char.IsDigit);
    private static bool ContainsSpecialChar(string input)
        => input.Any(ch => !char.IsLetterOrDigit(ch));

    public static PasswordStrength CalculateStrength(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return PasswordStrength.Weak;

        int score = 0;


        if (password.Length >= 8) score++;
        if (password.Length >= 12) score++;

        if (ContainsUpperCase(password)) score++;
        if (ContainsNumber(password)) score++;
        if (ContainsSpecialChar(password)) score++;

        return score switch
        {
            < 3 => PasswordStrength.Weak,
            3 => PasswordStrength.Medium,
            _ => PasswordStrength.Strong,
        };
    }

    public static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{salt}|{password}"));

        return $"{Convert.ToBase64String(hashBytes)}";
    }

    public static bool VerifyPassword(string inputPassword, string passwordHash, string salt)
    {
        var inputPasswordHash = HashPassword(inputPassword, salt);
        return passwordHash == inputPasswordHash;
    }
}
