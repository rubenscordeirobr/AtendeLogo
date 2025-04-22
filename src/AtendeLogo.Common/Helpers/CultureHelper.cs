using System.Diagnostics.CodeAnalysis;
using AtendeLogo.Common.Mappers;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.Common.Helpers;

public static class CultureHelper
{
    public const Culture DefaultCulture = Culture.EnUs;

    public static Culture GetCulture(string? cultureCode)
    {
        if (EnumUtils.TryParse(cultureCode, out Culture culture))
        {
            return culture;
        }
        return CultureMapper.MapCulture(cultureCode) ?? DefaultCulture;
    }

    public static Culture GetCultureFromUrl(string requestUrl)
    {
        var firstSegment = requestUrl?.Split('/').FirstOrDefault();
        return CultureMapper.MapCulture(firstSegment) ?? DefaultCulture;
    }

    public static bool IsCultureCodeSupported(
        [NotNullWhen(true)]
        string? cultureCode)
    {
        if (cultureCode is null || cultureCode.Length < 2)
            return false;

        var culture = CultureMapper.MapCulture(cultureCode);
        return culture != null;
    }

    public static string? ExtractCultureCodeFromUri(Uri uri)
    {
        if (uri is null)
            return null;

        return ExtractCultureTagFromPath(uri.AbsolutePath);
    }

    public static string? ExtractCultureTagFromPath(string path)
    {
        if (path is null)
            return null;

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length > 0 && IsCultureCodeSupported(segments[0]))
        {
            return segments[0];
        }
        return null;
    }
     
    public static string BuildDefaultCulturePathWithQuery(string path, string? queryString)
    {
        Guard.NotNullOrWhiteSpace(path);

        var defaultCultureCode = CultureMapper.MapCode(DefaultCulture);
        return BuildCulturePathWithQuery(defaultCultureCode, path, queryString);
    }

    public static string BuildCulturePathWithQuery(string cultureCode, string path, string? queryString)
    {
        Guard.NotNull(cultureCode);

        if (!IsCultureCodeSupported(cultureCode))
        {
            throw new InvalidOperationException(
                $"Culture '{cultureCode}' is not supported.");
        }

        var result = $"/{cultureCode.ToLowerInvariant()}/{path?.TrimEnd('/')}";
        if (!string.IsNullOrEmpty(queryString))
        {
            return $"{result}?{queryString}";
        }
        return result;
    }

    public static Country GetCountry(Culture culture)
    {
        return CultureMapper.MapCountry(culture);
    }

    public static Currency GetCurrency(Culture culture)
    {
        return CultureMapper.MapCurrency(culture);
    }

    public static Language GetLanguage(Culture culture)
    {
        return CultureMapper.MapLanguage(culture);
    }
}

