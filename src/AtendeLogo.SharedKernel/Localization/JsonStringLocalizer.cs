using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Shared.Localization;

public class JsonStringLocalizer<T> : JsonStringLocalizerBase, IJsonStringLocalizer<T>
{
    private readonly string _resourceKey;

    public JsonStringLocalizer(
        IJsonStringLocalizerCache localizationCache,
        ICultureProvider cultureProvider) 
        : base(localizationCache, cultureProvider)
    {
        _resourceKey = LocalizationHelper.GetResourceKey<T>();
    }
  
    protected override string GetLocalizedString(string localizationKey, string defaultValue, object[] args)
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
