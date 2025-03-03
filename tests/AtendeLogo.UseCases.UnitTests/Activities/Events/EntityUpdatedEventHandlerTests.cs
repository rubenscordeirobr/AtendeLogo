namespace AtendeLogo.UseCases.UnitTests.Activities.Events;

public class EntityUpdatedEventHandlerTests
    : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly Fixture _figure = new();
    private readonly IServiceProvider _serviceProvider;

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
        eventContext
            .ShouldHaveExecutedEvent(updatedEvent)
            .WithHandler<EntityUpdatedEventHandler<TenantUser>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldNotThrow()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var updatedEvent = new EntityUpdatedEvent<TenantUser>(entity, []);
        var activityRepository = new ActivityRepositoryMock();
        var userSessionAccessorMock = new AnonymousUserSessionAccessorMock();
        var logger = new LoggerServiceMock<EntityUpdatedEventHandler<TenantUser>>();

        var handler = new EntityUpdatedEventHandler<TenantUser>(
            activityRepository,
            userSessionAccessorMock,
            logger);

        // Act
        Func<Task> task = async () => await handler.HandleAsync(updatedEvent);

        // Assert
        await task.Should().NotThrowAsync();
    }
}
