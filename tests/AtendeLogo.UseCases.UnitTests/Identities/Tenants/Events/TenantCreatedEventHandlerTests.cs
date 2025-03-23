using AtendeLogo.Domain.Entities.Identities.Events;
using AtendeLogo.UseCases.Identities.Tenants.Events;

namespace AtendeLogo.UseCases.UnitTests.Identities.Tenants.Events;

public class TenantCreatedEventHandlerTests
     : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly Fixture _figure = new();
    private readonly IEventMediator _eventMediator;

    public TenantCreatedEventHandlerTests(
        AnonymousServiceProviderMock serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _eventMediator = serviceProviderMock.GetRequiredService<IEventMediator>();
    }

    [Fact]
    public async Task HandleAsync_ShouldLogInformation_WhenTenantCreatedEventIsHandled()
    {
        // Arrange
        var tenant = _figure.Create<Tenant>();
        var tenantUser = _figure.Create<TenantUser>();
        var createdEvent = new TenantCreatedEvent(tenant, tenantUser);
        var eventContext = new DomainEventContext([createdEvent]);

        // Act
        await _eventMediator.DispatchAsync(eventContext);

        // Assert
        eventContext
            .ShouldHaveExecutedEvent(createdEvent)
            .WithHandler<TenantCreatedEventHandler>();
    }
}
