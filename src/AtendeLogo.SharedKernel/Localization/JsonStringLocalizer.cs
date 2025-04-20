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
        ICultureProvider cultureProvider)
    {
        Guard.NotNull(localizationCache);
        Guard.NotNull(cultureProvider);

        _resourceKey = LocalizationHelper.GetResourceKey<T>();
        _cache = localizationCache;
        _cultureProvider = cultureProvider;
    }

    public string this[string localizationKey, string defaultValue, params object[] args]
    {
        get
        {
            var language = _cultureProvider.Language;
            var localizationString = _cache.GetLocalizedString(
                language,
                _resourceKey,
                localizationKey,
                defaultValue);

            return StringFormatUtils.Format(localizationString, args);
        }
    }

}
