using AtendeLogo.Common.Infos;
using AtendeLogo.Common.Resources;

namespace AtendeLogo.Common.Utils;

public static partial class PhoneNumberUtils
{
    public static Country GetCountryCode(string fullNumber)
    {
        var internationalDialingCode = GetInternationalDialingCode(fullNumber);
        return PhoneNumberUtilsInternal.GetCountryCodeInternal(
            internationalDialingCode,
            fullNumber);
    }

    public static string GetFullPhoneNumber(Country country, string nationalNumber)
    {
        var phoneFormatInfo = PhoneNumberFormatRegistry.TryGet(country);
        if (phoneFormatInfo == null)
        {
            throw new ArgumentException(
                $"Country '{country}' is not supported for phone number formatting.", nameof(country));
        }
         
        var cleanedNumber = nationalNumber.GetOnlyNumbers('+');
        if (cleanedNumber.Length < phoneFormatInfo.MinNationalNumberLength)
        {
            throw new ArgumentException(
                $"National number '{nationalNumber}' is too short. Minimum length required: {phoneFormatInfo.MinNationalNumberLength}.", nameof(nationalNumber));
        }
        
        // Check if the number already includes international format with "+"
        if(cleanedNumber.StartsWith('+'))
        {
            var providedDialingCode = cleanedNumber.SafeSubstring(1, phoneFormatInfo.InternationalDialingCodeLength);
            var expectedDialingCode = phoneFormatInfo.InternationalDialingCode.GetDialingCodeString();
            
            if (providedDialingCode != expectedDialingCode)
            {
                throw new ArgumentException(
                $"National number '{nationalNumber}' contains invalid international dialing code. Expected: +{expectedDialingCode}.", nameof(nationalNumber));
            }
            return cleanedNumber;
        }

        var dialingCode = phoneFormatInfo.InternationalDialingCode.GetDialingCodeString();
        return $"+{dialingCode}{cleanedNumber}";
    }

    public static InternationalDialingCode GetInternationalDialingCode(string fullNumber)
    {
        var numbers = fullNumber.GetOnlyNumbers('+');
        if (!numbers.StartsWith('+') || numbers.Length < 10)
        {
            return InternationalDialingCode.Unknown;
        }
        return PhoneNumberUtilsInternal.GetInternationalDialingCodeInternal(fullNumber);
    }

    public static bool IsFullPhoneNumberValid(string? fullPhoneNumber)
    {
        if (fullPhoneNumber == null)
        {
            return false;
        }

        var country = GetCountryCode(fullPhoneNumber);
        var phoneFormatInfo = PhoneNumberFormatRegistry.TryGet(country);
        if (phoneFormatInfo == null)
        {
            return false;
        }

        var numbers = fullPhoneNumber.GetOnlyNumbers();
        var nationalNumber = numbers.SafeSubstring(phoneFormatInfo.InternationalDialingCodeLength);
        return IsNationalNumberValid(phoneFormatInfo, nationalNumber);
    }

    public static bool IsNationalNumberValid(
        Country countryCode,
        string? nationalNumber)
    {
        if (countryCode == Country.Unknown)
            return false;

        if (string.IsNullOrEmpty(nationalNumber))
            return false;

        var phoneFormatInfo = PhoneNumberFormatRegistry.TryGet(countryCode);
        if (phoneFormatInfo == null)
        {
            return false;
        }
        return IsNationalNumberValid(phoneFormatInfo, nationalNumber);
    }

    private static bool IsNationalNumberValid(
        PhoneNumberFormatInfo phoneFormatInfo,
        string nationalNumber)
    {
        var minLength = phoneFormatInfo.MinNationalNumberLength;
        var maxLength = phoneFormatInfo.MaxNationalNumberLength;
        return nationalNumber.Length >= minLength
            && nationalNumber.Length <= maxLength;
    }
}

