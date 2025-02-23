using AtendeLogo.Application.UnitTests.Mocks.Extensions;
using AtendeLogo.UseCases;

namespace AtendeLogo.Application.UnitTests.Mocks;

public abstract class AbstractServiceProviderMock : IServiceProvider
{
    private IServiceProvider _serviceProvider;
    protected abstract bool IsAnonymous { get; }
    public AbstractServiceProviderMock()
    {
        _serviceProvider = new ServiceCollection()
              .AddApplicationServices()
              .AddLoggerServiceMock()
              .AddInMemoryIdentityDbContext()
              .AddMockInfrastructureServices(IsAnonymous)
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

public class TenantUserServiceProviderMock : AbstractServiceProviderMock
{
    protected override bool IsAnonymous => false;
}

public class AnonymousServiceProviderMock : AbstractServiceProviderMock
{
    protected override bool IsAnonymous => true;
   
}
