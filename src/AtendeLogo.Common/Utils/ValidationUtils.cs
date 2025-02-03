
using System.Text.RegularExpressions;

namespace AtendeLogo.Common.Utils;

public static class ValidationUtils
{
    private static readonly Regex HexRegex = new(@"^[a-fA-F0-9]+$", RegexOptions.Compiled);

    public static bool IsPhoneNumberValid(string? phoneNumber)
    {
        return PhoneNumberUtils.IsFullPhoneNumberValid(phoneNumber);
    }
    public static bool IsSha256(string? value)
    {
        if (value == null)
            return false;

        if (value.Length != 64)
            return false;

        return IsHex(value);
    }

    private static bool IsHex(string value)
    {
        return HexRegex.IsMatch(value);
    }
}
