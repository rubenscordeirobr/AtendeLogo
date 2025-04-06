using AtendeLogo.Common.Enums;

namespace AtendeLogo.Common.Mappers;

public static class LanguageMapper
{
    public static string MapTag(Language language)
    {
        return language switch
        {
            Language.Default => "en",
            Language.English => "en",
            Language.Spanish => "es",
            Language.French => "fr",
            Language.German => "de",
            Language.Portuguese => "pt",
            _ => throw new NotImplementedException($"Language {language} not implemented in LanguageMapper")

        };
    }

    public static Language? MapLanguage(string? languageTag)
    {
        return languageTag switch
        {
            "en" => Language.English,
            "es" => Language.Spanish,
            "fr" => Language.French,
            "de" => Language.German,
            "pt" => Language.Portuguese,
            _ => null
        };
    }
  
}
