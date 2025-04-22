using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Localization;

public abstract class JsonStringLocalizerBase : IJsonStringLocalizer
{
    internal readonly IJsonStringLocalizerCache Cache;
    internal readonly ICultureProvider CultureProvider;

    protected JsonStringLocalizerBase(
        IJsonStringLocalizerCache localizationCache,
        ICultureProvider cultureProvider)
    {
        Guard.NotNull(localizationCache);
        Guard.NotNull(cultureProvider);

        Cache = localizationCache;
        CultureProvider = cultureProvider;
    }

    public string this[string localizationKey, string defaultValue, params object[] args]
        => GetLocalizedString(localizationKey, defaultValue, args);

    protected abstract string GetLocalizedString(string localizationKey, string defaultValue, object[] args);
}
