using AtendeLogo.Common.Enums;

namespace AtendeLogo.RuntimeServices.Providers;

public class LanguageProvider : ILanguageProvider
{
    private readonly IHttpContextSessionAccessor _sessionAccessor;

    public LanguageProvider(IHttpContextSessionAccessor sessionAccessor)
    {
        // Constructor logic here
        _sessionAccessor = sessionAccessor;
    }

    public Language Language
        => _sessionAccessor.Language;

}

