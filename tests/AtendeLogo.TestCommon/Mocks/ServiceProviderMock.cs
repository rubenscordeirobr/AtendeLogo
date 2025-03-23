using AtendeLogo.Application;
using AtendeLogo.TestCommon.Extensions;
using AtendeLogo.UseCases;

namespace AtendeLogo.TestCommon.Mocks;
public abstract class ServiceProviderMock<TRoleProvider> : AbstractTestOutputServiceProvider
     where TRoleProvider : IRoleProvider, new()
{
    private readonly IServiceProvider _serviceProvider;
    protected UserRole UserRole { get; }

    protected override IServiceProvider ServiceProvider
        => _serviceProvider;
    protected ServiceProviderMock()
    {
        var services = new ServiceCollection()
              .AddApplicationServices()
              .AddLoggerServiceMock()
              .AddInMemoryIdentityDbContext()
              .AddMockInfrastructureServices<TRoleProvider>()
              .AddPersistenceServicesMock()
              .AddUserCasesServices()
              .AddUserCasesSharedServices();

        services.AddSingleton<ITestOutputHelper, TestOutputProxy>();

        _serviceProvider = services.BuildServiceProvider();
    }
}

public class TenantOwnerUserServiceProviderMock : ServiceProviderMock<TenantOwnerRole>
{
    public TenantOwnerUserServiceProviderMock()
    {
    }
}

public class AnonymousServiceProviderMock : ServiceProviderMock<AnonymousRole>
{

    public AnonymousServiceProviderMock()
    {
    }
}

public class SystemAdminUserServiceProviderMock : ServiceProviderMock<SystemAdminRole>
{
    public SystemAdminUserServiceProviderMock()
    {
    }
}
