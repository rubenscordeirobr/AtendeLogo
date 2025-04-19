using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Extensions;

public static class JsonStringLocalizerCacheExtensions
{
    public static Task LoadCultureAsync(
        this IJsonStringLocalizerCache localizerCache,
        string cultureCode)
    {
        Guard.NotNull(localizerCache);
        Guard.NotNullOrWhiteSpace(cultureCode);

        var culture = CultureHelper.GetCulture(cultureCode);
        return localizerCache.LoadCultureAsync(culture);
    }

    public static Task LoadDefaultCultureAsync( 
        this IJsonStringLocalizerCache localizerCache )
    {
        Guard.NotNull(localizerCache);
        return localizerCache.LoadCultureAsync(CultureHelper.DefaultCulture);
    }
}
