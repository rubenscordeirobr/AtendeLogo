using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.FunctionalTests.Identities;

public partial class TenantUserAuthenticationServiceTests
{
    [Fact]
    public async Task LogoutAsync_WhenAnonymousUser_ShouldBeFailure()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var command = new TenantUserLogoutCommand(sessionId);

        // Act
        var result = await _clientService.LogoutAsync(command);

        // Assert
        result.ShouldBeFailure<AuthenticationError>();
    }

    public class TheLogoutAsyncMethod_WithTenantOwnerLogged
        : IClassFixture<ClientServiceProviderMock<TenantOwnerRole>>
    {
        private readonly ITenantUserAuthenticationService _clientService;
        private readonly IServiceProvider _clientServiceProvider;

        public TheLogoutAsyncMethod_WithTenantOwnerLogged(
            ClientServiceProviderMock<TenantOwnerRole> serviceProviderMock,
            ITestOutputHelper testOutput)
        {
            serviceProviderMock.AddTestOutput(testOutput);

            _clientServiceProvider = serviceProviderMock; ;
            _clientService = serviceProviderMock.GetRequiredService<ITenantUserAuthenticationService>();
        }

        [Fact]
        public async Task LogoutAsync_WhenValidCredentials_ShouldBeSuccessful()
        {
            // Arrange
            var userSessionAccessor = _clientServiceProvider.GetRequiredService<IClientTenantUserSessionContext>();
            var userSession = userSessionAccessor.UserSession;

            var command = new TenantUserLogoutCommand(userSession!.Id);

            //// Act
            var result = await _clientService.LogoutAsync(command);

            //// Assert
            result.ShouldBeSuccessful();
        }

        [Fact]
        public async Task LogoutAsync_WhenInvalidSessionId_ShouldBeFailure()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new TenantUserLogoutCommand(sessionId);

            // Act
            var result = await _clientService.LogoutAsync(command);

            // Assert
            result.ShouldBeFailure<ForbiddenError>();
        }
    }
}
