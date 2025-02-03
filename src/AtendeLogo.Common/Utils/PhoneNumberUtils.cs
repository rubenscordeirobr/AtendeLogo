using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Common.Infos;
using AtendeLogo.Common.Resources;

namespace AtendeLogo.Common.Utils;
public static partial class PhoneNumberUtils
{
    public static CountryCode GetCountryCode(string fullNumber)
    {
        var onlyNumber = fullNumber.GetOnlyNumbers('+');
        var internationalDialingCode = GetInternationalDialingCode(fullNumber);
        return PhoneNumberUtilsInternal.GetCountryCodeInternal(
            internationalDialingCode,
            fullNumber);
    }

    public static InternationalDialingCode GetInternationalDialingCode(string fullNumber)
    {
        var numbers = fullNumber.GetOnlyNumbers('+');
        if (!numbers.StartsWith("+") || numbers.Length < 10)
        {
            return InternationalDialingCode.Unknown;
        }
        return PhoneNumberUtilsInternal.GetInternationalDialingCodeInternal(fullNumber);
    }
    public static bool IsFullPhoneNumberValid(string? fullPhoneNumber)
    {
        if(fullPhoneNumber == null)
        {
            return false;
        }

        var countryCode = GetCountryCode(fullPhoneNumber);
        var phoneFormatInfo = CountryPhoneFormatMetadata.TryGet(countryCode);
        if (phoneFormatInfo == null)
        {
            return false;
        }

        var numbers = fullPhoneNumber.GetOnlyNumbers();
        var nationalNumber = numbers.SafeSubstring(phoneFormatInfo.InternationalDialingCodeLength);
        return IsNationalNumberValid(phoneFormatInfo, nationalNumber);
    }
     
    public static bool IsNationalNumberValid(
        CountryCode countryCode,
        string nationalNumber)
    {
        if(countryCode == CountryCode.Unknown)
        {
            return false;
        }

        var phoneFormatInfo = CountryPhoneFormatMetadata.TryGet(countryCode);
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
        var minLenght = phoneFormatInfo.MinNumberLength;
        var maxLength = phoneFormatInfo.MaxNumberLength;
        return nationalNumber.Length >= minLenght
            && nationalNumber.Length <= maxLength;
    }
}
 
