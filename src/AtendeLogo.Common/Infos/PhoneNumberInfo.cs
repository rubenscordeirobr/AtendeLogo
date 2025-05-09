﻿using AtendeLogo.Common.Utils;

namespace AtendeLogo.Common.Infos;

public partial record PhoneNumberInfo(
    Country CountryCode,
    InternationalDialingCode InternationalDialingCode,
    string AreaCode,
    string NationalNumber,
    string FormattedNationalNumber)
{
    public string FullNumber => $"+{(int)InternationalDialingCode}{NationalNumber}";
}

public partial record PhoneNumberInfo
{
    public static PhoneNumberInfo Unknown(string numbers)
    {
        return new PhoneNumberInfo
        (
            CountryCode : Country.Unknown,
            InternationalDialingCode: InternationalDialingCode.Unknown,
            NationalNumber: numbers,
            AreaCode: string.Empty,
            FormattedNationalNumber: numbers
        );
    }

    public static Result<PhoneNumberInfo> Create (string fullPhoneNumber)
    {
        var phoneNumberInfo = PhoneNumberInfoParser.Parse(fullPhoneNumber);

        if (phoneNumberInfo.CountryCode == Country.Unknown)
        {
            return Result.ValidationFailure<PhoneNumberInfo>(
                "PhoneNumberInfo.InvalidCountryCode",
                "Country code is not valid.");
        }

        if (phoneNumberInfo.InternationalDialingCode == InternationalDialingCode.Unknown)
        {
            return Result.ValidationFailure<PhoneNumberInfo>(
                "PhoneNumberInfo.InvalidInternationalDialingCode",
                "International dialing code is not valid.");
        }

        if (string.IsNullOrEmpty(phoneNumberInfo.NationalNumber))
        {
            return Result.ValidationFailure<PhoneNumberInfo>(
                "PhoneNumberInfo.InvalidNationalNumber",
                "National number is not valid.");
        }

        var isNationalNumberValid = PhoneNumberUtils.IsNationalNumberValid(phoneNumberInfo.CountryCode, phoneNumberInfo.NationalNumber);
        if (!isNationalNumberValid)
        {
            return Result.ValidationFailure<PhoneNumberInfo>(
               "PhoneNumberInfo.InvalidNationalNumber",
               "National number is not valid.");
        }
        return Result.Success(phoneNumberInfo);
    }
}
