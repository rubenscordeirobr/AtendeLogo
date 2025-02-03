using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Infos;

namespace AtendeLogo.Common.Resources;

internal class PhoneFormatMetadata
{
    public static PhoneNumberFormatInfo? TryGet(CountryCode countryCode)
    {
        return Mapping.TryGetValue(countryCode, out var info) ? info : null;
    }

    private static readonly Dictionary<CountryCode, PhoneNumberFormatInfo> _mapping = new()
    {
        { CountryCode.USA,  new PhoneNumberFormatInfo {
            CountryCode = CountryCode.USA,
            InternationalDialingCode = InternationalDialingCode.UnitedStatesOrCanada,
            InternationalDialingCodeLength = 1,
            MinAreaCodeLength = 3,
            MaxAreaCodeLength = 3,
            MinNationalNumberLength = 10,  // 3 (area) + 7 (subscriber)
            MaxNationalNumberLength = 10,  // Fixed 10-digit length
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(###) ###-####",
        }},
        { CountryCode.CAN, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.CAN,
            InternationalDialingCode = InternationalDialingCode.UnitedStatesOrCanada,
            InternationalDialingCodeLength = 1,
            MinAreaCodeLength = 3,
            MaxAreaCodeLength = 3,
            MinNationalNumberLength = 10,  // 3 (area) + 7 (subscriber)
            MaxNationalNumberLength = 10,  // Fixed 10-digit length
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(###) ###-####",
        }},
        { CountryCode.MEX,  new PhoneNumberFormatInfo {
            CountryCode = CountryCode.MEX,
            InternationalDialingCode = InternationalDialingCode.Mexico,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 3,
            MinNationalNumberLength = 10,
            MaxNationalNumberLength = 11,
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(##) #### ####",  // Landline format
            AlternateNationalFormats = [("(###) #### ####", 11)]  // Mobile format
        }},
        { CountryCode.ARG, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.ARG,
            InternationalDialingCode = InternationalDialingCode.Argentina,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 4,
            MinNationalNumberLength = 10,  // (2-digit area code) + (8-digit number)
            MaxNationalNumberLength = 13,  // (4-digit area code) + (7-digit number, mobile)
            IsLeadingZeroNeeded = true,  // Leading "0" required for domestic calls
            PrimaryNationalFormat = "0## #### ####",  // Landline format
            AlternateNationalFormats = [("0### 15 #### ####", 13)]  // Mobile format
        }},
        { CountryCode.BOL, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.BOL,
            InternationalDialingCode = InternationalDialingCode.Bolivia,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 8,  // (2-digit area code) + (6-digit subscriber number)
            MaxNationalNumberLength = 9,  // (2-digit area code) + (7-digit subscriber number)
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(##) ######",  // Landline format
            AlternateNationalFormats = [("(##) #######", 9)]  // Mobile format
        }},
        { CountryCode.BRA, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.BRA,
            InternationalDialingCode = InternationalDialingCode.Brazil,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 10,  // (2-digit area code) + (8-digit landline number)
            MaxNationalNumberLength = 11,  // (2-digit area code) + (9-digit mobile number)
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(##) #####-####",  // Mobile format (most common)
            AlternateNationalFormats = [("(##) ####-####", 10) ]  // Landline format
        }},
        { CountryCode.CHL, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.CHL,
            InternationalDialingCode = InternationalDialingCode.Chile,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 9,
            MaxNationalNumberLength = 9,
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(##) #### ####"
        }},
        { CountryCode.COL, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.COL,
            InternationalDialingCode = InternationalDialingCode.Colombia,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 3,
            MaxAreaCodeLength = 3,
            MinNationalNumberLength = 10,
            MaxNationalNumberLength = 10,
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(###) #### ####"
        }},
        { CountryCode.ECU, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.ECU,
            InternationalDialingCode = InternationalDialingCode.Ecuador,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 9,
            MaxNationalNumberLength = 9,
            IsLeadingZeroNeeded = true,
            PrimaryNationalFormat = "0## #### ####"
        }},
        { CountryCode.GUY, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.GUY,
            InternationalDialingCode = InternationalDialingCode.Guyana,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 7,
            MaxNationalNumberLength = 7,
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(##) #######"
        }},
        { CountryCode.PER, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.PER,
            InternationalDialingCode = InternationalDialingCode.Peru,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 1,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 9,
            MaxNationalNumberLength = 9,
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(#) #### ####"
        }},
        { CountryCode.PRY, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.PRY,
            InternationalDialingCode = InternationalDialingCode.Paraguay,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 9,
            MaxNationalNumberLength = 9,
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(##) #### ####"
        }},
        { CountryCode.URY, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.URY,
            InternationalDialingCode = InternationalDialingCode.Uruguay,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 8,
            MaxNationalNumberLength = 8,
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(##) #### ####"
        }},
        { CountryCode.SUR, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.SUR,
            InternationalDialingCode = InternationalDialingCode.Suriname,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 1,
            MaxAreaCodeLength = 1,
            MinNationalNumberLength = 7,  // (1-digit area code) + (6-digit subscriber number)
            MaxNationalNumberLength = 7,  // Fixed length of 7 digits
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(#) ######"
         }},
        { CountryCode.VEN, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.VEN,
            InternationalDialingCode = InternationalDialingCode.Venezuela,
            InternationalDialingCodeLength = 3,
            MinAreaCodeLength = 3,
            MaxAreaCodeLength = 3,
            MinNationalNumberLength = 10,  // (3-digit area code) + (7-digit subscriber number)
            MaxNationalNumberLength = 10,  // Fixed 10-digit length
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "(###) #######"
        }},
        { CountryCode.ESP, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.ESP,
            InternationalDialingCode = InternationalDialingCode.Spain,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 3,
            MinNationalNumberLength = 9,  // Including area code
            MaxNationalNumberLength = 9,  // Fixed 9-digit length
            IsLeadingZeroNeeded = false,
            PrimaryNationalFormat = "### ### ###"
        }},
        { CountryCode.DEU, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.DEU,
            InternationalDialingCode = InternationalDialingCode.Germany,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 5,  // Germany has variable-length area codes
            MinNationalNumberLength = 10,  // Including area code
            MaxNationalNumberLength = 11,  // Some numbers have 11 digits total
            IsLeadingZeroNeeded = true,
            PrimaryNationalFormat = "0### #######",
            AlternateNationalFormats = [ ( "0## #######", 10), ( "0#### ######", 11) ]
        }},
        { CountryCode.FRA, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.FRA,
            InternationalDialingCode = InternationalDialingCode.France,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 1,
            MaxAreaCodeLength = 1,
            MinNationalNumberLength = 10,  // Fixed 10-digit length
            MaxNationalNumberLength = 10,
            IsLeadingZeroNeeded = true,
            PrimaryNationalFormat = "0# ## ## ## ##"
        }},
        { CountryCode.GBR, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.GBR,
            InternationalDialingCode = InternationalDialingCode.UnitedKingdom,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 5,  // UK has variable-length area codes
            MinNationalNumberLength = 10,  // Including area code
            MaxNationalNumberLength = 11,  // Some mobile numbers have 11 digits
            IsLeadingZeroNeeded = true,
            PrimaryNationalFormat = "0## #### ####",
            AlternateNationalFormats =   [( "0#### ######" , 11), ( "0### ### ####", 10)]
         }},
        { CountryCode.ITA, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.ITA,
            InternationalDialingCode = InternationalDialingCode.Italy,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 4,
            MinNationalNumberLength = 9,  // Including area code
            MaxNationalNumberLength = 11,  // Some mobile numbers have 11 digits
            IsLeadingZeroNeeded = true,
            PrimaryNationalFormat = "0## #### ####",
            AlternateNationalFormats = [ ("0### ### ####", 10), ("0#### ### ###", 10)]
         }},
        { CountryCode.PRT, new PhoneNumberFormatInfo {
            CountryCode = CountryCode.PRT,
            InternationalDialingCode = InternationalDialingCode.Portugal,
            InternationalDialingCodeLength = 2,
            MinAreaCodeLength = 2,
            MaxAreaCodeLength = 2,
            MinNationalNumberLength = 9,  // Fixed 9-digit length
            MaxNationalNumberLength = 9,
            IsLeadingZeroNeeded = true,
            PrimaryNationalFormat = "0## ### ####"
        }}
    };

    public static Dictionary<CountryCode, PhoneNumberFormatInfo> Mapping => _mapping;
}
