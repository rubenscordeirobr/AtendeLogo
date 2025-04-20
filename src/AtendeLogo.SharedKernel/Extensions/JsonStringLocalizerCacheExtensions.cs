using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Extensions;

public static class JsonStringLocalizerCacheExtensions
{
    public static Task LoadCultureAsync(
        this IJsonStringLocalizerCache localizerCache,
        string languageCode)
    {
        Guard.NotNull(localizerCache);
        Guard.NotNullOrWhiteSpace(languageCode);

        var language = LanguageHelper.GetLanguage(languageCode);
        return localizerCache.LoadLanguageAsync(language);
    }

    public static Task LoadDefaultCultureAsync( 
        this IJsonStringLocalizerCache localizerCache )
    {
        Guard.NotNull(localizerCache);
        return localizerCache.LoadLanguageAsync(LanguageHelper.DefaultLanguage);
    }
}
