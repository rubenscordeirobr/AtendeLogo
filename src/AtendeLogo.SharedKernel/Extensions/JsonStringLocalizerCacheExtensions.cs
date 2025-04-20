using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Extensions;

public static class JsonStringLocalizerCacheExtensions
{
    public static Task EnsureLanguageLoadedAsync(
        this IJsonStringLocalizerCache localizerCache,
        string languageCode)
    {
        Guard.NotNull(localizerCache);
        Guard.NotNullOrWhiteSpace(languageCode);

        var language = LanguageHelper.GetLanguage(languageCode);
        return localizerCache.EnsureLanguageLoadedAsync(language);
    }

    public static Task EnsureDefaultCultureAsync( 
        this IJsonStringLocalizerCache localizerCache )
    {
        Guard.NotNull(localizerCache);
        return localizerCache.EnsureLanguageLoadedAsync(LanguageHelper.DefaultLanguage);
    }
}
