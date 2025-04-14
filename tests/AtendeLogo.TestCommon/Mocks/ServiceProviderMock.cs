using AtendeLogo.Application;
using AtendeLogo.RuntimeServices;
using AtendeLogo.UseCases;
using AtendeLogo.Shared;
using AtendeLogo.TestCommon.Extensions;
using AtendeLogo.Shared.Helpers;
using AtendeLogo.Shared.Extensions;

namespace AtendeLogo.TestCommon.Mocks;
public class ServiceProviderMock<TRoleProvider> : AbstractTestOutputServiceProvider
     where TRoleProvider : IRoleProvider, new()
{
    private readonly IServiceProvider _serviceProvider;

    protected UserRole UserRole { get; }

    protected override IServiceProvider ServiceProvider
        => _serviceProvider;
     

    public ServiceProviderMock()
    {
        _serviceProvider = BuildServiceProvider();
    }

    private IServiceProvider BuildServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .AddSharedKernelServices()
            .AddApplicationServices()
            .AddLoggerServiceMock()
            .AddRuntimeServices()
            .AddMockInfrastructureServices()
            .AddInMemoryIdentityDbContext()
            .AddUserSessionAccessorMock<TRoleProvider>()
            .AddPersistenceServicesMock()
            .AddUserCasesServices()
            .AddUserCasesSharedServices()
            .AddSingleton<ITestOutputHelper, TestOutputProxy>()
            .BuildServiceProvider();

        InitializeJsonLocalizerCache(serviceProvider);
        return serviceProvider;
            
    }

    private void InitializeJsonLocalizerCache(ServiceProvider serviceProvider)
    {
        var LocalizerCache = serviceProvider.GetRequiredService<IJsonStringLocalizerCache>();
        var task = LocalizerCache.LoadDefaultLanguageAsync();
        task.Wait();
    }
}
