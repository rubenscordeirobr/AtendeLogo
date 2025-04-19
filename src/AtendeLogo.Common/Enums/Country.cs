using System.ComponentModel.DataAnnotations;

namespace AtendeLogo.Common.Enums;

public enum Country
{
    [UndefinedValue]
    Unknown = 0,

    [Display(Name = "United States")]
    [CountryNumericCode(840)]
    [CountryAbbreviation("USA")]

    UnitedStates,

    [Display(Name = "Canada")]
    [CountryNumericCode(124)]
    [CountryAbbreviation("CAN")]
    Canada,

    [Display(Name = "Mexico")]
    [CountryNumericCode(484)]
    [CountryAbbreviation("MEX")]
    Mexico,

    // South America
    [Display(Name = "Argentina")]
    [CountryNumericCode(32)]
    [CountryAbbreviation("ARG")]
    Argentina,

    [Display(Name = "Bolivia")]
    [CountryNumericCode(68)]
    [CountryAbbreviation("BOL")]
    Bolivia,

    [Display(Name = "Brazil")]
    [CountryNumericCode(76)]
    [CountryAbbreviation("BRA")]
    Brazil,

    [Display(Name = "Chile")]
    [CountryNumericCode(152)]
    [CountryAbbreviation("CHL")]
    Chile,

    [Display(Name = "Colombia")]
    [CountryNumericCode(170)]
    [CountryAbbreviation("COL")]
    Colombia,

    [Display(Name = "Ecuador")]
    [CountryNumericCode(218)]
    [CountryAbbreviation("ECU")]
    Ecuador,

    [Display(Name = "Guyana")]
    [CountryNumericCode(328)]
    [CountryAbbreviation("GUY")]
    Guyana,

    [Display(Name = "Peru")]
    [CountryNumericCode(604)]
    [CountryAbbreviation("PER")]
    Peru,

    [Display(Name = "Paraguay")]
    [CountryNumericCode(600)]
    [CountryAbbreviation("PRY")]
    Paraguay,

    [Display(Name = "Suriname")]
    [CountryNumericCode(740)]
    [CountryAbbreviation("SUR")]
    Suriname,

    [Display(Name = "Uruguay")]
    [CountryNumericCode(858)]
    [CountryAbbreviation("URY")]
    Uruguay,

    [Display(Name = "Venezuela")]
    [CountryNumericCode(862)]
    [CountryAbbreviation("VEN")]
    Venezuela,

    // Europe
    [Display(Name = "Spain")]
    [CountryNumericCode(724)]
    [CountryAbbreviation("ESP")]
    Spain,

    [Display(Name = "Germany")]
    [CountryNumericCode(276)]
    [CountryAbbreviation("DEU")]
    Germany,

    [Display(Name = "France")]
    [CountryNumericCode(250)]
    [CountryAbbreviation("FRA")]
    France,

    [Display(Name = "United Kingdom")]
    [CountryNumericCode(826)]
    [CountryAbbreviation("GBR")]
    UnitedKingdom,

    [Display(Name = "Italy")]
    [CountryNumericCode(380)]
    [CountryAbbreviation("ITA")]
    Italy,

    [Display(Name = "Portugal")]
    [CountryNumericCode(620)]
    [CountryAbbreviation("PRT")]
    Portugal,
}
