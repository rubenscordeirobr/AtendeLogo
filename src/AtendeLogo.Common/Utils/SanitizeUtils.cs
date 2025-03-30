﻿namespace AtendeLogo.Common.Utils;

public static class SanitizeUtils
{
    public static string SanitizeEmailOrPhoneNumber(string emailOrPhoneNumber)
    {
        if (string.IsNullOrEmpty(emailOrPhoneNumber))
        {
            return string.Empty;
        }

        if (ValidationUtils.IsEmail(emailOrPhoneNumber))
        {
            return SanitizeEmail(emailOrPhoneNumber);
        }

        if (ValidationUtils.IsFullPhoneNumberValid(emailOrPhoneNumber))
        {
            return SanitizePhoneNumber(emailOrPhoneNumber);
        }

        return emailOrPhoneNumber.Trim();

    }

    public static string SanitizeEmail(string emailOrPhoneNumber)
    {
        if (string.IsNullOrEmpty(emailOrPhoneNumber))
            return string.Empty;

        return emailOrPhoneNumber.Trim().ToLowerInvariant();
    }

    public static string SanitizePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return string.Empty;

        return phoneNumber.GetOnlyNumbers('+');
    }

    public static string SanitizeFiscalCode(string fiscalCode)
    {
        if (string.IsNullOrEmpty(fiscalCode))
            return string.Empty;

        return fiscalCode.GetOnlyNumbers();
    }
}

