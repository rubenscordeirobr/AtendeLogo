using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Extensions;

public static class JsonStringLocalizerCacheExtensions
{
    public static Task EnsureSystemLanguageLoadedAsync( 
        this IJsonStringLocalizerCache localizerCache )
    {
        Guard.NotNull(localizerCache);
        return localizerCache.EnsureLanguageLoadedAsync(LanguageHelper.SystemLanguage);
    }
}
