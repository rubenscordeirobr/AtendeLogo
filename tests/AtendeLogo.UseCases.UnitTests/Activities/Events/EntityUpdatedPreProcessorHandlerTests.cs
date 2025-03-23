using AtendeLogo.Application.Mediators;

namespace AtendeLogo.UseCases.UnitTests.Activities.Events;

public class EntityUpdatedPreProcessorHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly Fixture _figure = new();
    private readonly IServiceProvider _serviceProvider;

    public EntityUpdatedPreProcessorHandlerTests(
        AnonymousServiceProviderMock serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _serviceProvider = serviceProviderMock;
    }

    [Fact]
    public async Task EventMediator_ShouldDispatch()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var eventMediator = _serviceProvider.GetRequiredService<IEventMediator>();
        var updateEvent = new EntityUpdatedEvent<TenantUser>(entity, []);
        var eventContext = new DomainEventContext([updateEvent]);
       
        // Act
        await eventMediator.PreProcessorDispatchAsync(eventContext);

        // Assert
        eventContext
            .ShouldHaveExecutedEvent(updateEvent)
            .WithHandler<EntityUpdatedPreProcessorHandler<TenantUser>>();
    }
     
    [Fact]
    public async Task PreProcessAsync_ShouldNotThrow()
    {
        // Arrange
        var entity = _figure.Create<TenantUser>();
        var updateEvent = new EntityUpdatedEvent<TenantUser>(entity, []);
        var eventContext = new DomainEventContext([updateEvent]);
        var eventData = DomainEventDataFactory.Create(eventContext, updateEvent);
        var handler = new EntityUpdatedPreProcessorHandler<TenantUser>();

        // Act
        Func<Task> task = async () => await handler.PreProcessAsync(eventData);
      
        // Assert
        await task.Should()
            .NotThrowAsync();
    }
}
