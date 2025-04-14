namespace AtendeLogo.Shared.Abstractions;

public interface IJsonStringLocalizerCache
{
    string GetLocalizedString(
        Language language,
        string resourceIdentifier,
        string localizationKey,
        string defaultValue);

    Task LoadLanguageAsync(Language language);

}
