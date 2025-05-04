using AtendeLogo.Shared.Extensions;

namespace AtendeLogo.UI.Services;

public class RouteService : IRouteService
{
    private readonly ICultureProvider _cultureProvider;

    public RouteService(ICultureProvider cultureProvider)
    {
        _cultureProvider = cultureProvider;
    }

    public string BuildLocalizedRoute(string route)
    {
        return $"{_cultureProvider.GetCultureCode()}/{route}";
    }
}
