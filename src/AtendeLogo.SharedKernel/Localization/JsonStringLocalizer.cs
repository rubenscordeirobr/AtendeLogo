using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Shared.Localization;

public class JsonStringLocalizer<T> : IJsonStringLocalizer<T>
{
    private readonly IJsonStringLocalizerCache _cache;
    private readonly ICultureProvider _cultureProvider;

    private readonly string _resourceKey;

    public JsonStringLocalizer(
        IJsonStringLocalizerCache localizationCache,
        ICultureProvider localizationProvider)
    {
        Guard.NotNull(localizationCache);
        Guard.NotNull(localizationProvider);

        _resourceKey = LocalizationHelper.GetResourceKey<T>();
        _cache = localizationCache;
        _cultureProvider = localizationProvider;
    }

    public string this[string localizationKey, string defaultValue, params object[] args]
    {
        get
        {
            var culture = _cultureProvider.Culture;
            var localizationString = _cache.GetLocalizedString(
                culture,
                _resourceKey,
                localizationKey,
                defaultValue);

            return StringFormatUtils.Format(localizationString, args);
        }
    }

}
