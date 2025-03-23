namespace AtendeLogo.UseCases.UnitTests.Activities.Events;

public class EntityDeletedEventHandlerTests
    : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly Fixture _figure = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutput;

    public EntityDeletedEventHandlerTests(AnonymousServiceProviderMock serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _serviceProvider = serviceProviderMock;
        _testOutput = testOutput;
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
        eventContext
            .ShouldHaveExecutedEvent(deletedEvent)
            .WithHandler<EntityDeletedEventHandler<TenantUser>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldNotThrow()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var deletedEvent = new EntityDeletedEvent<TenantUser>(entity, []);
        var activityRepository = new ActivityRepositoryMock();
        var userSessionAccessorMock = new AnonymousUserSessionAccessorMock();
        var logger = new TestOutputLogger<EntityDeletedEventHandler<TenantUser>>(_testOutput);

        var handler = new EntityDeletedEventHandler<TenantUser>(
            activityRepository,
            userSessionAccessorMock,
            logger);

        // Act
        Func<Task> task = async () => await handler.HandleAsync(deletedEvent);

        // Assert
        await task.Should().NotThrowAsync();
    }
}
