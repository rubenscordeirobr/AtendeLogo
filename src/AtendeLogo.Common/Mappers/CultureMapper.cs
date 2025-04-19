using AtendeLogo.Common.Enums;

namespace AtendeLogo.Common.Mappers;

public static class CultureMapper
{
    public static string MapCode(Culture culture)
    {
        return culture switch
        {
            Culture.Default => "en-us",
            // North America
            Culture.EnUs => "en-us",
            Culture.EnCa => "en-ca",
            Culture.EsMx => "es-mx",

            // South America
            Culture.EsAr => "es-ar",
            Culture.EsBo => "es-bo",
            Culture.PtBr => "pt-br",
            Culture.EsCl => "es-cl",
            Culture.EsCo => "es-co",
            Culture.EsEc => "es-ec",
            Culture.EnGy => "en-gy",
            Culture.EsPe => "es-pe",
            Culture.GnPy => "gn-py",
            Culture.NlSr => "nl-sr",
            Culture.EsUy => "es-uy",
            Culture.EsVe => "es-ve",

            // Europe
            Culture.EsEs => "es-es",
            Culture.DeDe => "de-de",
            Culture.FrFr => "fr-fr",
            Culture.EnGb => "en-gb",
            Culture.ItIt => "it-it",
            Culture.PtPt => "pt-pt",

            _ => throw new NotImplementedException($"Culture {culture} not implemented in CultureMapper")

        };
    }

    public static Culture? MapCulture(string? cultureCode)
    {
        return cultureCode?.ToLowerInvariant() switch
        {
            // North America
            "en-us" => Culture.EnUs,
            "en-ca" => Culture.EnCa,
            "es-mx" => Culture.EsMx,

            // South America
            "es-ar" => Culture.EsAr,
            "es-bo" => Culture.EsBo,
            "pt-br" => Culture.PtBr,
            "es-cl" => Culture.EsCl,
            "es-co" => Culture.EsCo,
            "es-ec" => Culture.EsEc,
            "en-gy" => Culture.EnGy,
            "es-pe" => Culture.EsPe,
            "gn-py" => Culture.GnPy,
            "nl-sr" => Culture.NlSr,
            "es-uy" => Culture.EsUy,
            "es-ve" => Culture.EsVe,

            // Europe
            "es-es" => Culture.EsEs,
            "de-de" => Culture.DeDe,
            "fr-fr" => Culture.FrFr,
            "en-gb" => Culture.EnGb,
            "it-it" => Culture.ItIt,
            "pt-pt" => Culture.PtPt,
            _ => null
        };
    }
  
}
