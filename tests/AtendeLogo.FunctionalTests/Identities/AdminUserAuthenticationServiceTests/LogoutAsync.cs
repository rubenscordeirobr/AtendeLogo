﻿using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.FunctionalTests.Identities;

public partial class AdminUserAuthenticationServiceTests
{
    [Fact]
    public async Task LogoutAsync_WhenAnonymousUser_ShouldBeFailure()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var command = new AdminUserLogoutCommand(sessionId);

        // Act
        var result = await _clientService.LogoutAsync(command);

        // Assert
        result.ShouldBeFailureForErrors<AuthenticationError, ForbiddenError>();
    }

    public class TheLogoutAsyncMethod_WithAdminUserLogged
        : IClassFixture<ClientServiceProviderMock<AdminUserRole>>
    {
        private readonly IAdminUserAuthenticationService _clientService;
        private readonly IServiceProvider _clientServiceProvider;

        public TheLogoutAsyncMethod_WithAdminUserLogged(
            ClientServiceProviderMock<AdminUserRole> serviceProviderMock,
            ITestOutputHelper testOutput)
        {
            serviceProviderMock.AddTestOutput(testOutput);

            _clientServiceProvider = serviceProviderMock;
            _clientService = serviceProviderMock.GetRequiredService<IAdminUserAuthenticationService>();
        }

        [Fact]
        public async Task LogoutAsync_WhenValidCredentials_ShouldBeSuccessful()
        {
            // Arrange
            var userSessionAccessor = _clientServiceProvider.GetRequiredService<IClientAdminUserSessionContext>();
            var userSession = userSessionAccessor.UserSession;

            var command = new AdminUserLogoutCommand(userSession!.Id);

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
            var command = new AdminUserLogoutCommand(sessionId);

            // Act
            var result = await _clientService.LogoutAsync(command);

            // Assert
            result.ShouldBeFailure<ForbiddenError>();
        }
    }
}
