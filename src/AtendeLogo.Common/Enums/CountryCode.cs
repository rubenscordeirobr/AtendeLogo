using System.ComponentModel.DataAnnotations;
 
namespace AtendeLogo.Common.Enums;

public enum CountryCode
{
    Unknown =0,

    // North America
    [Display(Name = "United States")]
    USA = 840,

    [Display(Name = "Canada")]
    CAN = 124,

    [Display(Name = "Mexico")]
    MEX = 484,

    // South America
    [Display(Name = "Argentina")]
    ARG = 32,

    [Display(Name = "Bolivia")]
    BOL = 68,

    [Display(Name = "Brazil")]
    BRA = 76,

    [Display(Name = "Chile")]
    CHL = 152,

    [Display(Name = "Colombia")]
    COL = 170,

    [Display(Name = "Ecuador")]
    ECU = 218,

    [Display(Name = "Guyana")]
    GUY = 328,

    [Display(Name = "Peru")]
    PER = 604,

    [Display(Name = "Paraguay")]
    PRY = 600,

    [Display(Name = "Suriname")]
    SUR = 740,

    [Display(Name = "Uruguay")]
    URY = 858,

    [Display(Name = "Venezuela")]
    VEN = 862,
     
    // Europe
    [Display(Name = "Spain")]
    ESP = 724,

    [Display(Name = "Germany")]
    DEU = 276,

    [Display(Name = "France")]
    FRA = 250,

    [Display(Name = "United Kingdom")]
    GBR = 826,

    [Display(Name = "Italy")]
    ITA = 380,

    [Display(Name = "Portugal")]
    PRT = 620,
}
