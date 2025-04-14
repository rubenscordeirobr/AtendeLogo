using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Shared.Extensions;

public static class JsonStringLocalizerCacheExtensions
{
    public static Task LoadLanguageAsync(
        this IJsonStringLocalizerCache localizerCache,
        string language)
    {
        Guard.NotNull(localizerCache);
        Guard.NotNullOrWhiteSpace(language);

        var languageEnum = LanguageHelper.GetLanguageEnum(language);
        return localizerCache.LoadLanguageAsync(languageEnum);
    }

    public static Task LoadDefaultLanguageAsync( 
        this IJsonStringLocalizerCache localizerCache )
    {
        Guard.NotNull(localizerCache);
        return localizerCache.LoadLanguageAsync(LanguageHelper.DefaultLanguage);
    }
}
