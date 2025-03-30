using AtendeLogo.UseCases.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[AllowAnonymous]
[EndPoint(RouteConstants.Api)]
public class ApiEndpoint : ApiEndpointBase, IApiService
{
    [HttpGet(routeTemplate: RouteConstants.Version)]
    public string GetVersion()
    {
        return "0.0.1";
    }
}

