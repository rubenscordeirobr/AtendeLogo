using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Mappers;

namespace AtendeLogo.RuntimeServices.Providers;

public class CultureProvider : ICultureProvider
{
    private readonly IHttpContextSessionAccessor _sessionAccessor;

    public CultureProvider(IHttpContextSessionAccessor sessionAccessor)
    {
        _sessionAccessor = sessionAccessor;
    }

    public Culture Culture
        => _sessionAccessor.Culture;

    public Currency Currency
        => CultureMapper.MapCurrency(Culture);

    public Country Country
        => CultureMapper.MapCountry(Culture);

    public Language Language
        => _sessionAccessor.Language;
}

