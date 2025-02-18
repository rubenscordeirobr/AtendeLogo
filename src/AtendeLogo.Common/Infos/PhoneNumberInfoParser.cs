using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Resources;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.Common.Infos;
public static partial class PhoneNumberInfoParser
{
    public static PhoneNumberInfo Parse(string fullNumber)
    {
        if (string.IsNullOrWhiteSpace(fullNumber))
            return PhoneNumberInfo.Unknown(string.Empty);
         
        var numbers = fullNumber.GetOnlyNumbers('+');
        if (!numbers.StartsWith("+"))
        {
            return PhoneNumberInfo.Unknown(numbers);
        }

        var internationalDialingCode = PhoneNumberUtils.GetInternationalDialingCode(numbers);
        var countryCode = PhoneNumberUtilsInternal.GetCountryCodeInternal(internationalDialingCode, numbers);
        var countryMetedataInfo = PhoneFormatMetadata.TryGet(countryCode);

        if (countryCode == Country.Unknown ||
            internationalDialingCode == InternationalDialingCode.Unknown ||
            countryMetedataInfo == null)
        {
            return PhoneNumberInfo.Unknown(numbers);
        }

        var nationalNumber = numbers.Substring(1 + countryMetedataInfo.InternationalDialingCodeLength);
        var codeAreaLength = countryMetedataInfo.GetBetterAreaCodeLength(nationalNumber.Length);
        var areaCode = nationalNumber.SafeTrim(codeAreaLength);
        var formatterNumber = PhoneNumberUtilsInternal.FormatNatianalNumber(countryMetedataInfo, nationalNumber);

        return new PhoneNumberInfo
        (
            CountryCode: countryCode,
            InternationalDialingCode: internationalDialingCode,
            NationalNumber: nationalNumber,
            AreaCode: areaCode,
            FormattedNationalNumber: formatterNumber
        );
    }
}

