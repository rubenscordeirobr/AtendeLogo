using System.ComponentModel;
using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Common.Infos;

namespace AtendeLogo.Common.Utils;

internal static partial class PhoneNumberUtilsInternal
{
    private static readonly HashSet<string> _canadianAreaCodes = new()
        {
          "204", "226", "236", "249", "250", "289", "306", "343", "365", "387",
          "403", "416", "418", "431", "437", "438", "450", "506", "514", "519",
          "548", "579", "581", "587", "600", "604", "613", "639", "647", "672",
          "705", "709", "742", "778", "780", "782", "807", "819", "825", "867",
          "873", "902", "905"
        };

    private static readonly Dictionary<InternationalDialingCode, CountryCode> _mappings = new() {
            { InternationalDialingCode.Unknown, CountryCode.Unknown },
            { InternationalDialingCode.Mexico, CountryCode.MEX },
            { InternationalDialingCode.Argentina, CountryCode.ARG },
            { InternationalDialingCode.Bolivia, CountryCode.BOL },
            { InternationalDialingCode.Brazil, CountryCode.BRA },
            { InternationalDialingCode.Chile, CountryCode.CHL },
            { InternationalDialingCode.Colombia, CountryCode.COL },
            { InternationalDialingCode.Ecuador, CountryCode.ECU },
            { InternationalDialingCode.Guyana, CountryCode.GUY },
            { InternationalDialingCode.Paraguay, CountryCode.PRY },
            { InternationalDialingCode.Peru, CountryCode.PER },
            { InternationalDialingCode.Suriname, CountryCode.SUR },
            { InternationalDialingCode.Uruguay, CountryCode.URY },
            { InternationalDialingCode.Venezuela, CountryCode.VEN },
            { InternationalDialingCode.Italy, CountryCode.ITA },
            { InternationalDialingCode.France, CountryCode.FRA },
            { InternationalDialingCode.Germany, CountryCode.DEU },
            { InternationalDialingCode.Spain, CountryCode.ESP },
            { InternationalDialingCode.Portugal, CountryCode.PRT },
            { InternationalDialingCode.UnitedKingdom, CountryCode.GBR }
        };

    internal static string FormatNatianalNumber(
        PhoneNumberFormatInfo? countryMetedataInfo,
        string nationalNumber)
    {
        if (countryMetedataInfo == null)
        {
            return nationalNumber;
        }

        if (countryMetedataInfo.IsLeadingZeroNeeded)
        {
            nationalNumber = $"0{nationalNumber}";
        }

        var provider = new MaskedTextProvider(countryMetedataInfo.NationalFormat, true);
        var isSucess = provider.Set(nationalNumber);
        if (!isSucess)
        {
            return nationalNumber;
        }
        return provider.ToDisplayString();
    }

    internal static CountryCode GetCountryCodeInternal(
        InternationalDialingCode internationalDialingCode,
        string fullNumber)
    {
        if (internationalDialingCode == InternationalDialingCode.UnitedStatesOrCanada)
        {
            var areaCode = fullNumber.GetOnlyNumbers().Substring(1, 3);
            if (_canadianAreaCodes.Contains(areaCode))
            {
                return CountryCode.CAN;
            }
            return CountryCode.USA;
        }

        return _mappings.TryGetValue(internationalDialingCode, out var countryCode)
             ? countryCode
             : CountryCode.Unknown;
    }

    internal static InternationalDialingCode GetInternationalDialingCodeInternal(string numbers)
    {
        numbers = numbers.Substring(1);

        if (EnumUtils.TryParse<InternationalDialingCode>(numbers[..3], out var result) )
        {
            return result;
        }

        if (EnumUtils.TryParse<InternationalDialingCode>(numbers[..2], out var result2))
        {
            return result2;
        }

        if (numbers.StartsWith("1"))
        {
            return InternationalDialingCode.UnitedStatesOrCanada;
        }
        return InternationalDialingCode.Unknown;
    }
}
