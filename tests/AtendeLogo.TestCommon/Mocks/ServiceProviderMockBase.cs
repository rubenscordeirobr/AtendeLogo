using AtendeLogo.Application;
using AtendeLogo.TestCommon.Extensions;
using AtendeLogo.UseCases;

namespace AtendeLogo.TestCommon.Mocks;
public abstract class ServiceProviderMockBase : AbstractTestOutputServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    protected abstract UserRole UserRole { get; }

    protected override IServiceProvider ServiceProvider
        => _serviceProvider;
    protected ServiceProviderMockBase()
    {
        var services = new ServiceCollection()
              .AddApplicationServices()
              .AddLoggerServiceMock()
              .AddInMemoryIdentityDbContext()
              .AddMockInfrastructureServices(UserRole)
              .AddPersistenceServicesMock()
              .AddUserCasesServices()
              .AddUserCasesSharedServices();

        services.AddSingleton<ITestOutputHelper, TestOutputProxy>();

        _serviceProvider = services.BuildServiceProvider();
    }
}

public class TenantOwnerUserServiceProviderMock : ServiceProviderMockBase
{
    protected override UserRole UserRole => UserRole.Owner;
    public TenantOwnerUserServiceProviderMock()
    {
    }
}

public class AnonymousServiceProviderMock : ServiceProviderMockBase
{
    protected override UserRole UserRole => UserRole.Anonymous;

    public AnonymousServiceProviderMock()
    {
    }
}

public class SystemAdminUserServiceProviderMock : ServiceProviderMockBase
{
    protected override UserRole UserRole => UserRole.SystemAdmin;

    public SystemAdminUserServiceProviderMock()
    {
    }
}
