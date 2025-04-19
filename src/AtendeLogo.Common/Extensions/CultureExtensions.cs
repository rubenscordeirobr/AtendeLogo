using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Common.Mappers;

namespace AtendeLogo.Common.Extensions;

public static class CultureExtensions
{
    public static bool IsDefaultCulture(this Culture culture)
    {
        return culture == Culture.Default || 
            culture == CultureHelper.DefaultCulture;
    }

    public static string GetCultureCode(this Culture culture)
    {
        return CultureMapper.MapCode(culture);
    }
}
