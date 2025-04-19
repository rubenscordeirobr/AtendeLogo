
using AtendeLogo.Common.Mappers;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.Shared.Helpers;

public static class LanguageHelper
{
    public const Language DefaultLanguage = Language.English;

    public static Language GetLanguageEnum(string? languageRef)
    {
        if (EnumUtils.TryParse(languageRef, out Language language))
        {
            return language;
        }
        return LanguageMapper.MapLanguage(languageRef) ?? Language.Default;
    }

    public static Language? GetLanguageFromUrl(string requestUrl)
    {
        var firstSegment = requestUrl?.Split('/').FirstOrDefault();
        return LanguageMapper.MapLanguage(firstSegment);
    }

    public static bool IsLangTagSupported(string? languageTag)
    {
        if (languageTag is null || languageTag.Length != 2)
            return false;

        var language = LanguageMapper.MapLanguage(languageTag);
        return language != null;
    }

    public static string? ExtractLanguageTagFromUri(Uri uri)
    {
        if (uri is null)
            return null;

        return ExtractLanguageTagFromPath(uri.AbsolutePath);
    }

    public static string? ExtractLanguageTagFromPath(string path)
    {
        if (path is null)
            return null;

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length > 0 && IsLangTagSupported(segments[0]))
        {
            return segments[0];
        }
        return null;
    }

    internal static Language Normalize(Language language)
    {
        return (language == Language.Default)
            ?  DefaultLanguage
            : language;
    }
    public static string BuildDefaultLanguagePathWithQuery(string path, string? queryString)
    {
        Guard.NotNullOrWhiteSpace(path);

        var defaultLanguageTag = LanguageMapper.MapTag(DefaultLanguage);
        return BuildLanguagePathWithQuery(defaultLanguageTag, path, queryString);
    }

    public static string BuildLanguagePathWithQuery(string languageTag, string path, string? queryString)
    {
        if (!IsLangTagSupported(languageTag))
        {
            throw new InvalidOperationException(
                $"Language '{languageTag}' is not supported.");
        }

        var result = $"/{languageTag}/{path?.TrimEnd('/')}";
        if (!string.IsNullOrEmpty(queryString))
        {
            return $"{result}?{queryString}";
        }
        return result;

    }
}

