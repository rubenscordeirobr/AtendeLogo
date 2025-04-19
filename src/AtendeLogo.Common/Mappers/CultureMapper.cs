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

    public static Country MapCountry(Culture culture)
    {
        culture = CultureHelper.Normalize(culture);
        return culture switch
        {
            // North America
            Culture.EnUs => Country.UnitedStates,
            Culture.EnCa => Country.Canada,
            Culture.EsMx => Country.Mexico,
            // South America
            Culture.EsAr => Country.Argentina,
            Culture.EsBo => Country.Bolivia,
            Culture.PtBr => Country.Brazil,
            Culture.EsCl => Country.Chile,
            Culture.EsCo => Country.Colombia,
            Culture.EsEc => Country.Ecuador,
            Culture.EnGy => Country.Guyana,
            Culture.EsPe => Country.Peru,
            Culture.GnPy => Country.Paraguay,
            Culture.NlSr => Country.Suriname,
            Culture.EsUy => Country.Uruguay,
            Culture.EsVe => Country.Venezuela,
            // Europe
            Culture.EsEs => Country.Spain,
            Culture.DeDe => Country.Germany,
            Culture.FrFr => Country.France,
            Culture.EnGb => Country.UnitedKingdom,
            Culture.ItIt => Country.Italy,
            Culture.PtPt => Country.Portugal,
            _ => throw new NotImplementedException($"Country for culture {culture} not implemented in CultureMapper")
        };
    }

    public static Currency MapCurrency(Culture culture)
    {
        return culture switch
        {
            // North America
            Culture.EnUs => Currency.USD,
            Culture.EnCa => Currency.USD, // Canada officially uses CAD, but assuming USD for simplicity
            Culture.EsMx => Currency.USD,
          
            // South America
            Culture.EsAr => Currency.USD, // Argentina uses ARS
            Culture.EsBo => Currency.USD, // Bolivia uses BOB
            Culture.PtBr => Currency.BRL,
            Culture.EsCl => Currency.USD, // Chile uses CLP
            Culture.EsCo => Currency.USD, // Colombia uses COP
            Culture.EsEc => Currency.USD,
            Culture.EnGy => Currency.USD, // Guyana uses GYD
            Culture.EsPe => Currency.USD, // Peru uses PEN
            Culture.GnPy => Currency.USD, // Paraguay uses PYG
            Culture.NlSr => Currency.USD, // Suriname uses SRD
            Culture.EsUy => Currency.USD, // Uruguay uses UYU
            Culture.EsVe => Currency.USD, // Venezuela uses VES

            // Europe
            Culture.EsEs => Currency.EUR,
            Culture.DeDe => Currency.EUR,
            Culture.FrFr => Currency.EUR,
            Culture.EnGb => Currency.EUR, // UK uses GBP; fallback to EUR
            Culture.ItIt => Currency.EUR,
            Culture.PtPt => Currency.EUR,
            _ => throw new NotImplementedException($"Currency for culture {culture} not implemented in CultureMapper")
        };

    }
}
