namespace AtendeLogo.Application.UnitTests.UseCases.Activities.Events;

public class EntityCreatedEventHandlerTests
    : IClassFixture<AnonymousServiceProviderMock>
{
    private Fixture _figure = new();

    private IServiceProvider _serviceProvider;

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

        var executedEvents = eventContext.GetExecutedEventResults(createdEvent);
        var handlers = executedEvents.Select(x => x.Handler).ToList();

        handlers.Should()
            .ContainItemsAssignableTo<EntityCreatedEventHandler<TenantUser>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldNotThrow()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var createdEvent = new EntityCreatedEvent<TenantUser>(entity, []);
        var activityRepository = new ActivityRepositoryMock();
        var userSessionServiceMock = new AnonymousUserSessionServiceMock();

        var handler = new EntityCreatedEventHandler<TenantUser>(
            activityRepository,
            userSessionServiceMock,
            new LoggerServiceMock<EntityCreatedEventHandler<TenantUser>>());

        // Act
        Func<Task> task = async () => await handler.HandleAsync(createdEvent);

        // Assert
        await task.Should().NotThrowAsync();
    }
}
