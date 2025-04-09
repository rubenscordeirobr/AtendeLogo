using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Shared.Localization;

public class JsonStringLocalizer<T> : IJsonStringLocalizer<T>
{
    private readonly IJsonStringLocalizerCache _cache;
    private readonly ILanguageProvider _languageProvider;

    private readonly string _resourceKey;

    public JsonStringLocalizer(
        IJsonStringLocalizerCache localizationCache,
        ILanguageProvider localizationProvider)
    {
        Guard.NotNull(localizationCache);
        Guard.NotNull(localizationProvider);

        _resourceKey = LocalizationHelper.GetResourceKey<T>();
        _cache = localizationCache;
        _languageProvider = localizationProvider;
    }

    public string this[string localizationKey, string defaultValue, params object[] args]
    {
        get
        {
            var language = _languageProvider.GetLanguage();
            var localizationString = _cache.GetLocalizedString(
                language,
                _resourceKey,
                localizationKey,
                defaultValue);

            return StringFormatUtils.Format(localizationString, args);
        }
    }

}
