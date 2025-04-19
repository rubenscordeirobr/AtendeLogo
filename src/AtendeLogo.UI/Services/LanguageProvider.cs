namespace AtendeLogo.UI.Services;

public class LanguageProvider : ILanguageProvider
{
    private Language _language = LanguageHelper.DefaultLanguage;

    private readonly IJsonStringLocalizerCache _stringLocalizerCache;

    public LanguageProvider(IJsonStringLocalizerCache stringLocalizerCache)
    {
        _stringLocalizerCache = stringLocalizerCache;
    }

    public Language Language
        => _language;
   
    public async Task SetLanguageAsync(string language)
    {
        _language = LanguageHelper.GetLanguageEnum(language);
        await _stringLocalizerCache.LoadLanguageAsync(language);
    }
}
