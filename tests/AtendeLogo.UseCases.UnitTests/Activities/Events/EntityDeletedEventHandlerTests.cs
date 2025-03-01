﻿namespace AtendeLogo.UseCases.UnitTests.Activities.Events;

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
        var userSessionServiceMock = new AnonymousUserSessionServiceMock();
        var logger = new LoggerServiceMock<EntityDeletedEventHandler<TenantUser>>();
         
        var handler = new EntityDeletedEventHandler<TenantUser>(
            activityRepository,
            userSessionServiceMock,
            logger);
     
        // Act
        Func<Task> task = async () => await handler.HandleAsync(deletedEvent);
     
        // Assert
        await task.Should().NotThrowAsync();
    }
}
