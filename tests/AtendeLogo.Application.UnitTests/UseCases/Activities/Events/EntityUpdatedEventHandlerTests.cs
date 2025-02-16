namespace AtendeLogo.Application.UnitTests.UseCases.Activities.Events;

public class EntityUpdatedEventHandlerTests
    : IClassFixture<AnonymousServiceProviderMock>
{
    private Fixture _figure = new();
    private IServiceProvider _serviceProvider;
    public EntityUpdatedEventHandlerTests(AnonymousServiceProviderMock serviceProviderMock)
    {
        _serviceProvider = serviceProviderMock;
    }

    [Fact]
    public async Task EventMediator_ShouldDispatch()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var eventMediator = _serviceProvider.GetRequiredService<IEventMediator>();
        var updatedEvent = new EntityUpdatedEvent<TenantUser>(entity, []);
        var eventContext = new DomainEventContext([updatedEvent]);
        // Act
        await eventMediator.DispatchAsync(eventContext);
        // Assert
        var executedEvents = eventContext.GetExecutedEventResults(updatedEvent);
        var handlers = executedEvents.Select(x => x.Handler).ToList();
        handlers.Should()
            .ContainItemsAssignableTo<EntityUpdatedEventHandler<TenantUser>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldNotThrow()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var updatedEvent = new EntityUpdatedEvent<TenantUser>(entity, []);
        var activityRepository = new ActivityRepositoryMock();
        var userSessionServiceMock = new AnonymousUserSessionServiceMock();
        var handler = new EntityUpdatedEventHandler<TenantUser>(
            activityRepository,
            userSessionServiceMock,
            new LoggerServiceMock<EntityUpdatedEventHandler<TenantUser>>());
       
        // Act
        Func<Task> task = async () => await handler.HandleAsync(updatedEvent);
        
        // Assert
        await task.Should().NotThrowAsync();
    }
}


