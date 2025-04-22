using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Localization;

namespace AtendeLogo.Shared.Extensions;

public static class JsonStringLocalizerExtensions
{
    public static IJsonStringLocalizer<TEnum> ForEnum<TEnum>(this IJsonStringLocalizer localizer)
        where TEnum : struct, Enum
    {
        if(localizer is not  JsonStringLocalizerBase localizerBase)
        {
            throw new ArgumentException(
                $"The {nameof(localizer)} must derive from {nameof(JsonStringLocalizerBase)}");
        }
        return new JsonStringLocalizer<TEnum>(localizerBase.Cache, localizerBase.CultureProvider);
    }
}
