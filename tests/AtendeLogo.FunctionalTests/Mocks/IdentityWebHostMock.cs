using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace AtendeLogo.FunctionalTests.Mocks;
public class IdentityWebHostMock<TRoleProvider>
    : WebApplicationFactory<Program>, IAsyncLifetime
    where TRoleProvider : IRoleProvider, new()
{
    public IdentityWebHostMock()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_SUB_ENVIRONMENT", "Test");
    }

    public void AddTestOutput(ITestOutputHelper output)
    {
        var serviceProvider = Services;
        var service = (TestOutputProxy)serviceProvider.GetRequiredService<ITestOutputHelper>()!;
        service.AddTestOutput(output);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddLoggerServiceMock()
                .AddMockInfrastructureServices()
                .AddPersistenceServicesMock()
                .AddInMemoryIdentityDbContext();

            //services.Remove<IUserSessionAccessor>();
            //services.Remove<IUserSessionVerificationService>();
            //services.AddSingleton<IUserSessionVerificationService, UserSessionVerificationServiceMock>();
            services.AddSingleton<ITestOutputHelper, TestOutputProxy>();
        });
    }
     
    #region IAsyncLifetime
    public Task InitializeAsync()
    {
        return Task.CompletedTask;

    }
    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
    }
    #endregion

}

//private async Task SaveUserSessionAsync()
//{
//    await using (var scope = Services.CreateAsyncScope())
//    {
//        var userSessionAccessor = scope.ServiceProvider.GetRequiredService<IUserSessionAccessor>();
//        var unitOfWork = scope.ServiceProvider.GetRequiredService<IIdentityUnitOfWork>();
//        var userSession = userSessionAccessor.GetCurrentSession();

//        var exists = await unitOfWork.UserSessions.ExistsAsync(userSession.Id);
//        if (!exists)
//        {
//            //await unitOfWork.SaveChangesAsync(silent: false);
//        }
//    }
//}
