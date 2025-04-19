using AtendeLogo.Common.Enums;

namespace AtendeLogo.RuntimeServices.Providers;

public class CultureProvider : ICultureProvider
{
    private readonly IHttpContextSessionAccessor _sessionAccessor;

    public CultureProvider(IHttpContextSessionAccessor sessionAccessor)
    {
        // Constructor logic here
        _sessionAccessor = sessionAccessor;
    }

    public Culture Culture
        => _sessionAccessor.Culture;

    public Currency Currency
        => throw new NotImplementedException();

    public Country Country 
        => throw new NotImplementedException();
}

