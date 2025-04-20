
using AtendeLogo.Common.Mappers;

namespace AtendeLogo.Common.Helpers;

public static class LanguageHelper
{
    public static Language DefaultLanguage => Language.English;
     
    public static Language Normalize(Language language)
    {
        return language == Language.Default
          ? DefaultLanguage
          : language;
    }

    public static Language GetLanguage(string? languageCode)
    {
        return LanguageMapper.MapLanguage(languageCode);
    }
}
