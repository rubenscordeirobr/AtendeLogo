using System.ComponentModel.DataAnnotations;

namespace AtendeLogo.Common.Enums;

public enum Language
{
    Default,

    [Display(Name = "Portuguese (Brazil)")]
    PortugueseBrazil,

    [Display(Name = "Portuguese (Portugal)")]
    PortuguesePortugal,

    [Display(Name = "English (Standard)")]
    English,

    [Display(Name = "Spanish (Latin America)")]
    LatinSpanish,

    [Display(Name = "Spanish (Spain)")]
    Spanish,

    [Display(Name = "French")]
    French,

    [Display(Name = "German")]
    German,

    [Display(Name = "Italian")]
    Italian,
}
