using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Shared.Abstractions;

public interface IJsonStringLocalizerCache
{
    string GetLocalizedString(
        Language language,
        string resourceIdentifier,
        string localizationKey,
        string defaultValue);

    Task InitializeAsync(Language language);

    public Task InitializeAsync(string language)
    {
        var languageEnum = LanguageHelper.GetLanguageEnum(language);
        return InitializeAsync(languageEnum);
    }
}
