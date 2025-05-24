using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Shared.Localization;

public class JsonStringLocalizer<T> : JsonStringLocalizerBase, IJsonStringLocalizer<T>
{
    public JsonStringLocalizer(
        IJsonStringLocalizerCache localizationCache,
        ICultureProvider cultureProvider)
        : base(localizationCache, cultureProvider, LocalizationHelper.GetResourceKey<T>())
    {
    }

    public string this[T @enum]
    {
        get
        {
            if (@enum is Enum enumValue)
            {
                var key = enumValue.ToString();
                var defaultValue = enumValue.GetDescription();
                return GetLocalizedString(key, defaultValue, []);
            }

            throw new NotSupportedException(
                $"Localization key '{@enum}' is not supported. It must be an enum value." +
                $"Use this[string localizationKey, string defaultValue, params object[] args) instead.");
        }
    }
}
