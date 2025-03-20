using AtendeLogo.Application;
using AtendeLogo.TestCommon.Extensions;
using AtendeLogo.UseCases;

namespace AtendeLogo.TestCommon.Mocks;

public abstract class AbstractServiceProviderMock : IServiceProvider
{
    private IServiceProvider _serviceProvider;
    protected abstract UserRole UserRole { get; }
    public AbstractServiceProviderMock()
    {
        _serviceProvider = new ServiceCollection()
              .AddApplicationServices()
              .AddLoggerServiceMock()
              .AddInMemoryIdentityDbContext()
              .AddMockInfrastructureServices(UserRole)
              .AddPersistenceServicesMock()
              .AddUserCasesServices()
              .AddUserCasesSharedServices()
              .BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}

public class TenantOwnerUserServiceProviderMock : AbstractServiceProviderMock
{
    protected override UserRole UserRole => UserRole.Owner;
}

public class AnonymousServiceProviderMock : AbstractServiceProviderMock
{
    protected override UserRole UserRole => UserRole.Anonymous;
}

public class SystemAdminUserServiceProviderMock : AbstractServiceProviderMock
{
    protected override UserRole UserRole => UserRole.SystemAdmin;
}
