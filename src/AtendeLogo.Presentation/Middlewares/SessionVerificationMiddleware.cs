using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Presentation.Middlewares;

public class SessionVerificationMiddleware
{
    private readonly RequestDelegate _next;
    
    public SessionVerificationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await using (var scope = context.RequestServices.CreateAsyncScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SessionVerificationMiddleware>>();
            try
            {
                var sessionVerification = scope.ServiceProvider.GetService<IUserSessionVerificationService>();
                if(sessionVerification == null)
                {
                    logger.LogError("UserSessionVerificationService not registered.");
                    return;
                }
                await sessionVerification.VerifyAsync();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error retrieving UserSessionVerificationService.");
            }
        }
        await _next(context);
    }
}
