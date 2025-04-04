using AtendeLogo.Application.Abstractions.Services;
using AtendeLogo.TestCommon.Mocks.Infrastructure;

namespace AtendeLogo.FunctionalTests.Identities;

 
public partial class TenantUserAuthenticationServiceTests
    : IClassFixture<ClientServiceProviderMock<AnonymousRole>>
{
    private readonly ITenantUserAuthenticationService _clientService;
    private readonly IServiceProvider _hostServiceProvider;

    public TenantUserAuthenticationServiceTests(
        ClientServiceProviderMock<AnonymousRole> serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _clientService = serviceProviderMock.GetRequiredService<ITenantUserAuthenticationService>();
        _hostServiceProvider = serviceProviderMock.HostServiceProvider;
    }
     
    private void ClearCache()
    {
        var cacheRepository = (CacheRepositoryMock)_hostServiceProvider.GetRequiredService<ICacheRepository>();
        cacheRepository.Clear("authentication-attempt-limiter");
    }

}
