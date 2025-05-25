using AtendeLogo.TenantPortal.BlazorServer.Services;

namespace AtendeLogo.TenantPortal.BlazorServer.Extensions;

internal static class MapRouteExtensions
{
    public static void MapTenantPortalBlazorServerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapNotFoundEndpoint();
        endpoints.MapSingInEndpoint();
    }

    public static void MapNotFoundEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapFallback(async context =>
        {
            var cultureCode = LocalizedUriUtils.ExtractCultureCodeFromUri(context.Request.Path);
            if (cultureCode is null)
            {
                var defaultCulturePath = LocalizedUriUtils.BuildDefaultCultureUri(
                    context.Request.Path,
                    context.Request.QueryString.ToString());

                context.Response.Redirect(defaultCulturePath);
                return;
            }

            var localizerCache = context.RequestServices.GetRequiredService<IJsonStringLocalizerCache>();
            var culture = CultureHelper.GetCulture(cultureCode);
            await localizerCache.EnsureLanguageLoadedAsync(culture);

            var route = StringFormatUtils.Format(RouteBlazorServerConstants.NotFoundRoute,
                cultureCode,
                context.Request.Path);

            context.Response.Redirect(route);
            await context.Response.CompleteAsync();
        });
    }

    public static void MapSingInEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(RouteBlazorServerConstants.BlazorServerLoginRoute, async context =>
        {
            var singInService = context.RequestServices.GetRequiredService<HttpContextSignInService>();
            await singInService.LoginAsync();
        });

        endpoints.MapPost(RouteBlazorServerConstants.BlazorServerLogoutRoute, async context =>
        {
            var singInService = context.RequestServices.GetRequiredService<HttpContextSignInService>();
            await singInService.LogoutAsync();
        });
    }
}
