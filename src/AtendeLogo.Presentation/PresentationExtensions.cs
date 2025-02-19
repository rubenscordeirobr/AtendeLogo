using AtendeLogo.Presentation.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AtendeLogo.Presentation;

public static class PresentationExtensions
{
    public static IApplicationBuilder UsePresentationServices(this IApplicationBuilder app)
    {
        app.UseMiddleware<SessionVerificationMiddleware>();
        return app;
    }
}

