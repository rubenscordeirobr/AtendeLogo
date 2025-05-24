using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Localization;

public abstract class JsonStringLocalizerBase : IJsonStringLocalizer
{
    internal readonly IJsonStringLocalizerCache Cache;
    internal readonly ICultureProvider CultureProvider;
    private readonly string _resourceKey;

    protected JsonStringLocalizerBase(
        IJsonStringLocalizerCache localizationCache,
        ICultureProvider cultureProvider,
        string resourceKey)
    {
        Guard.NotNull(localizationCache);
        Guard.NotNull(cultureProvider);

        Cache = localizationCache;
        CultureProvider = cultureProvider;
        _resourceKey = resourceKey;
    }

    public string this[string localizationKey, string defaultValue, params object[] args]
        => GetLocalizedString(localizationKey, defaultValue, args);

    protected string GetLocalizedString(string localizationKey, string defaultValue, object[] args)
    {
        var language = CultureProvider.Language;
        var localizationString = Cache.GetLocalizedString(
            language,
            _resourceKey,
            localizationKey,
            defaultValue);

        return StringFormatUtils.Format(localizationString, args);
    }
}
