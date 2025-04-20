using AtendeLogo.Common.Mappers;

namespace AtendeLogo.Common.Extensions;

public static class LanguageExtensions
{
    public static bool IsDefaultLanguage(this Language language)
    {
        return language == Language.Default ||
            language ==  LanguageHelper.DefaultLanguage;
    }

    public static string GetLanguageCode(this Language language)
    {
        return LanguageMapper.MapCode(language);
    }
}
