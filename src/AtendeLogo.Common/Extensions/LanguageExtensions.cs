using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Mappers;

namespace AtendeLogo.Common.Extensions;

public static class LanguageExtensions
{
    public static bool IsDefaultLanguage(this Language language)
    {
        return language == Language.Default || language == Language.English;
    }

    public static string GetLanguageTag(this Language language)
    {
        return LanguageMapper.MapTag(language);
    }
}
