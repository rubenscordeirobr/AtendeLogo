﻿using System.Diagnostics;
using AtendeLogo.Shared.Enums;
using AtendeLogo.UseCases.Identities.Authentications.Commands;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.FunctionalTests.Mocks;

public class ClientServiceProviderMock<TRoleProvider>
    : AbstractTestOutputServiceProvider, IAsyncLifetime
    where TRoleProvider : IRoleProvider, new()
{
    private readonly IdentityWebHostMock<TRoleProvider> _host;
    private readonly IServiceProvider _serviceProvider;

    protected override IServiceProvider ServiceProvider
        => _serviceProvider;

    public IServiceProvider HostServiceProvider
        => _host.Services;

    public ClientServiceProviderMock()
    {
        _host = new IdentityWebHostMock<TRoleProvider>();

        var servicesCollection = new ServiceCollection()
            .AddClientGatewayServices()
            .AddSingleton(typeof(ILogger<>), typeof(TestOutputLogger<>))
            .AddSingleton<IInternetStatusService, InternetStatusServiceMock>()
            .AddSingleton<IConnectionStatusNotifier, ConnectionStatusNotifierMock>()
            .AddSingleton<ITestOutputHelper, TestOutputProxy>()
            .AddSingleton<IClientAuthorizationTokenManager, ClientAuthorizationTokenManagerMock>()
            .AddSingleton<IClientTenantUserSessionContext, ClientTenantUserSessionContextMock>()
            .AddSingleton<IClientAdminUserSessionContext, ClientAdminUserSessionContextMock>()
            .AddSingleton<IClientApplicationInfo, ClientApplicationInfoMock>()
            .AddTransient(x => CreateClient());

        _serviceProvider = servicesCollection.BuildServiceProvider();
    }

    private HttpClient CreateClient()
    {
        var httpClient = _host.CreateClient();
        if (Debugger.IsAttached)
        {
            httpClient.Timeout = TimeSpan.FromMinutes(20);
        }
        return httpClient;
    }

    public async Task InitializeAsync()
    {
        await _host.InitializeAsync();

        var roleProvider = new TRoleProvider();
        switch (roleProvider.UserRole)
        {
            case UserRole.Owner:
                await LoginSystemTenantOwnerAsync();
                break;
            case UserRole.Admin:
                await LoginAdminUserAsync();
                break;
            case UserRole.Anonymous:
                //do nothing
                break;
            default:
                throw new NotImplementedException($"Login for UserRole {roleProvider.UserRole} is not implemented");

        }

    }
     
    private async Task LoginSystemTenantOwnerAsync()
    {
        var authenticationService = _serviceProvider.GetRequiredService<ITenantUserAuthenticationService>();
        var email = SystemTenantConstants.Email;
        var testPassword = SystemTenantConstants.TestPassword;

        var command = new TenantUserLoginCommand
        {
            EmailOrPhoneNumber = email,
            Password = testPassword,
            KeepSession = true
        };

        var result = await authenticationService.LoginAsync(command);
        if (result.IsFailure)
        {
            throw new Exception($"TenantOwner Login failed: {result.Error}");
        }
    }
    private async Task LoginAdminUserAsync()
    {
        var authenticationService = _serviceProvider.GetRequiredService<IAdminUserAuthenticationService>();
        var email = DefaultAdminUserConstants.Email;
        var testPassword = DefaultAdminUserConstants.TestPassword;

        var command = new AdminUserLoginCommand
        {
            EmailOrPhoneNumber = email,
            Password = testPassword,
            KeepSession = true
        };

        var result = await authenticationService.LoginAsync(command);
        if (result.IsFailure)
        {
            throw new Exception($"TenantOwner Login failed: {result.Error}");
        }
    }

    public async Task DisposeAsync()
    {
        await _host.DisposeAsync();
    }

    public override void AddTestOutput(ITestOutputHelper output)
    {
        _host.AddTestOutput(output);
        base.AddTestOutput(output);
    }
}
