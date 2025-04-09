using AtendeLogo.Common.Enums;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.RuntimeServices.Providers;

public class LanguageProvider : ILanguageProvider
{
    private readonly IHttpContextSessionAccessor _sessionAccessor;
    private Language? _language;

    public LanguageProvider(IHttpContextSessionAccessor sessionAccessor)
    {
        // Constructor logic here
        _sessionAccessor = sessionAccessor;
    }

    public Language GetLanguage()
    {
        if (!_language.HasValue)
        {
            _language = GetLanguageInternal();
        }
        return _language.Value;
    }

    private Language GetLanguageInternal()
    {
        var language = LanguageHelper.GetLanguageFromUrl(_sessionAccessor.RequestUrl);
        if (language.HasValue)
        {
            return language.Value;
        }
        return _sessionAccessor.UserSession?.Language ?? Common.Enums.Language.Default;
    }
}

