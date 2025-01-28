namespace AtendeLogo.Shared.Contantes;

public static class ValidationConstants
{
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 255;
    public const int EmailMaxLength = 100;
    public const int PhoneNumberMaxLength = 20;

    // User
    public const int PasswordMinLength = 6;
    public const int PasswordMaxLength = 50;

    // Tenant
    public const int CurrencyMaxLength = 3;
    public const int LanguageMaxLength = 5; //pt-BR
    public const int FiscalCodeMaxLength = 20;
    public const int DefaultTimeZoneMaxLength = 50;

    // Address
    public const int AddressMaxLength = 255;
    public const int AddressComplementMaxLength = 100;
    public const int CityMaxLength = 100;
    public const int StateMaxLength = 2;
    public const int CountryMaxLength = 2;
    public const int ZipCodeMaxLength = 10;
    public const int NumberMaxLength = 10;
    public const int NeighborhoodMaxLength = 100;
}

