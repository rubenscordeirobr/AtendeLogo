namespace AtendeLogo.Shared.Abstractions;

public interface IJsonStringLocalizerCache
{
    string GetLocalizedString(
        Culture culture,
        string resourceIdentifier,
        string localizationKey,
        string defaultValue);

    Task LoadCultureAsync(Culture culture);

}
