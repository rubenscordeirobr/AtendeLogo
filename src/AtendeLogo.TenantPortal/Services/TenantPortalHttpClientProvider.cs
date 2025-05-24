using System.Diagnostics;
using AtendeLogo.ClientGateway.Common.Abstractions;
using Microsoft.Extensions.Hosting;

namespace AtendeLogo.TenantPortal.Services;

internal sealed class TenantPortalHttpClientProvider : IHttpClientProvider, IDisposable
{
    private const int TIMEOUT_SECONDS_DEVELOPMENT = 30;
    private const int TIMEOUT_SECONDS_PRODUCTION = 15;
    private const int TIMEOUT_MINUTES_DEBUG_ATTACHED = 60;

    private const string ApiBaseAddressProject = "https://localhost:7241/";
    private const string ApiBaseAddressDockerCompose = "https://localhost:5001/";

    private HttpClient _apiHttpClient;

    public async Task InitializeAsync(IHostEnvironment environment)
    {
        var baseAddress = await GetIdentityApiAddressAsync(environment);
        var timeout = GetTimeout(environment);

        _apiHttpClient = new HttpClient
        {
            BaseAddress = new(baseAddress),
            Timeout = timeout
        };
    }

    public HttpClient GetHttpClient<T>()
    {
        return _apiHttpClient;
    }

    private async Task<string> GetIdentityApiAddressAsync(IHostEnvironment environment)
    {
        if (environment.IsAspire())
        {
            return ApiBaseAddressProject;
        }
#if DEBUG

        return await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            ApiBaseAddressProject,
            ApiBaseAddressDockerCompose);
#endif

        throw new NotImplementedException();
    }

    private TimeSpan GetTimeout(IHostEnvironment environment)
    {
        if (Debugger.IsAttached)
        {
            return TimeSpan.FromMinutes(TIMEOUT_MINUTES_DEBUG_ATTACHED);
        }

        if (environment.IsDevelopment())
        {
            return TimeSpan.FromSeconds(TIMEOUT_SECONDS_DEVELOPMENT);
        }
        return TimeSpan.FromSeconds(TIMEOUT_SECONDS_PRODUCTION);
    }

    public void Dispose()
    {
        _apiHttpClient.Dispose();
    }
}
