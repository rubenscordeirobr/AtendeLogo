namespace AtendeLogo.UseCases.UnitTests.Activities.Events;

public class EntityCreatedEventHandlerTests
    : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly Fixture _figure = new();
    private readonly IServiceProvider _serviceProvider;

    public EntityCreatedEventHandlerTests(AnonymousServiceProviderMock serviceProviderMock)
    {
        _serviceProvider = serviceProviderMock;
    }
     
    [Fact]
    public async Task EventMediator_ShouldDispatch()
    {
        // Arrange
        
        var entity = _figure.Create<TenantUser>();

        var eventMediator = _serviceProvider.GetRequiredService<IEventMediator>();

        var createdEvent = new EntityCreatedEvent<TenantUser>(entity, []);
        var eventContext = new DomainEventContext([createdEvent]);

        // Act
        await eventMediator.DispatchAsync(eventContext);

        // Assert
        eventContext
            .ShouldHaveExecutedEvent(createdEvent)
            .WithHandler<EntityCreatedEventHandler<TenantUser>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldNotThrow()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var createdEvent = new EntityCreatedEvent<TenantUser>(entity, []);
        var activityRepository = new ActivityRepositoryMock();
        var userSessionAccessorMock = new AnonymousUserSessionAccessorMock();
        var logger = new LoggerServiceMock<EntityCreatedEventHandler<TenantUser>>();

        var handler = new EntityCreatedEventHandler<TenantUser>(
            activityRepository,
            userSessionAccessorMock,
            logger);

        // Act
        Func<Task> task = async () => await handler.HandleAsync(createdEvent);

        // Assert
        await task.Should().NotThrowAsync();
    }
}
