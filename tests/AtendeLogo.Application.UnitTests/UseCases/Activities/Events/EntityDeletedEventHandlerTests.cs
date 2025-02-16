namespace AtendeLogo.Application.UnitTests.UseCases.Activities.Events;

public class EntityDeletedEventHandlerTests
    : IClassFixture<AnonymousServiceProviderMock>
{
    private Fixture _figure = new();
    private IServiceProvider _serviceProvider;

    public EntityDeletedEventHandlerTests(AnonymousServiceProviderMock serviceProviderMock)
    {
        _serviceProvider = serviceProviderMock;
    }

    [Fact]
    public async Task EventMediator_ShouldDispatch()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var eventMediator = _serviceProvider.GetRequiredService<IEventMediator>();
        var deletedEvent = new EntityDeletedEvent<TenantUser>(entity, []);
        var eventContext = new DomainEventContext([deletedEvent]);
        // Act
        await eventMediator.DispatchAsync(eventContext);
        // Assert
        var executedEvents = eventContext.GetExecutedEventResults(deletedEvent);
        var handlers = executedEvents.Select(x => x.Handler).ToList();
      
        handlers.Should()
            .ContainItemsAssignableTo<EntityDeletedEventHandler<TenantUser>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldNotThrow()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var deletedEvent = new EntityDeletedEvent<TenantUser>(entity, []);
        var activityRepository = new ActivityRepositoryMock();
        var userSessionServiceMock = new AnonymousUserSessionServiceMock();
     
        var handler = new EntityDeletedEventHandler<TenantUser>(
            activityRepository,
            userSessionServiceMock,
            new LoggerServiceMock<EntityDeletedEventHandler<TenantUser>>());
     
        // Act
        Func<Task> task = async () => await handler.HandleAsync(deletedEvent);
     
        // Assert
        await task.Should().NotThrowAsync();
    }
}
