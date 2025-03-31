using AtendeLogo.Application.Contracts.Events;
using AtendeLogo.Application.Contracts.Registrars;
using AtendeLogo.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AtendeLogo.Application.UnitTests.Mediators;

public class EventMediatorTests
{
    [Fact]
    public async Task EventMediator_ShouldDispatchEvent()
    {
        // Arrange
        var session = AnonymousUserConstants.AnonymousUserSession;
        var types = new Type[] { typeof(MockDomainEventHandler) };
        var eventMediator = CreateEventMediator(types);
        var domainEvent = new MockDomainEvent();
        var domainEventContext = new DomainEventContext(session, [domainEvent]);

        // Act
        await eventMediator.DispatchAsync(domainEventContext);

        // Assert
        domainEventContext.Error
            .Should().BeNull();

        var executedEvents = domainEventContext.GetExecutedEventResults(domainEvent);

        executedEvents.Should()
            .HaveCount(1);

        executedEvents.Should()
            .ContainSingle(x => x.Handler != null && x.Handler!.GetType() == typeof(MockDomainEventHandler));
    }

    [Fact]
    public async Task EventMediator_ShouldDispatchMultiEvent()
    {
        // Arrange
        var types = new Type[] {
            typeof(MockDomainEventHandler),
            typeof(MockDomainEvent2Handler)
        };

        var eventMediator = CreateEventMediator(types);

        var @event = new MockDomainEvent();
        var session = AnonymousUserConstants.AnonymousUserSession;
        var domainEventContext = new DomainEventContext(session, [@event]);

        // Act
        await eventMediator.DispatchAsync(domainEventContext);

        // Assert
        domainEventContext.Error
            .Should()
            .BeNull();

        var executedEvents = domainEventContext.GetExecutedEventResults(@event);
        executedEvents.Should()
            .HaveCount(2);

        executedEvents.Should()
            .ContainSingle(x => x.Handler != null && x.Handler!.GetType() == typeof(MockDomainEventHandler));

        executedEvents.Should()
           .ContainSingle(x => x.Handler != null && x.Handler!.GetType() == typeof(MockDomainEvent2Handler));
    }

    [Fact]
    public async Task EventMediator_ShouldHandlePreProcessor()
    {
        // Arrange
        var types = new Type[] { typeof(MockDomainEventPreProcessorHandler) };
        var eventMediator = CreateEventMediator(types);
        var domainEvent = new MockDomainEvent();
        var session = AnonymousUserConstants.AnonymousUserSession;
        var domainEventContext = new DomainEventContext(session, [domainEvent]);

        // Act
        await eventMediator.PreProcessorDispatchAsync(domainEventContext);

        // Assert
        domainEventContext.Error
            .Should()
            .BeNull();

        var executed = domainEventContext.GetExecutedEventResults(domainEvent);

        executed.Should()
            .HaveCount(1);

        executed.Should()
            .ContainSingle(x => x.Handler != null && x.Handler!.GetType() == typeof(MockDomainEventPreProcessorHandler));
    }

    [Fact]
    public async Task EventMediator_ShouldHandleCanceledPreProcessor()
    {
        // Arrange
        var types = new Type[] { typeof(MockDomainEventPreProcessorCanceledHandler) };
        var eventMediator = CreateEventMediator(types);
        var domainEvent = new MockDomainEvent();
        var session = AnonymousUserConstants.AnonymousUserSession;
        var domainEventContext = new DomainEventContext(session, [domainEvent]);

        // Act
        await eventMediator.PreProcessorDispatchAsync(domainEventContext);

        // Assert

        domainEventContext.IsCanceled
            .Should()
            .BeTrue();

        domainEventContext.Error
            .Should()
            .NotBeNull();

        domainEventContext.Error!.Message
            .Should()
            .Be("Cancelled");

        var executed = domainEventContext.GetExecutedEventResults(domainEvent);

        executed.Should()
            .HaveCount(0);
    }

    [Fact]
    public async Task EventMediator_ShouldTrowDomainEventCanceledExcpetionWhenHandleCancelledPreProcessor()
    {
        // Arrange
        var types = new Type[] { typeof(MockDomainEventPreProcessorCanceledHandler) };
        var eventMediator = CreateEventMediator(types);
        var domainEvent = new MockDomainEvent();

        var session = AnonymousUserConstants.AnonymousUserSession;
        var domainEventContext = new DomainEventContext(session, [domainEvent]);
        await eventMediator.PreProcessorDispatchAsync(domainEventContext);

        // Act
        Func<Task> act  = async () => await eventMediator.DispatchAsync(domainEventContext);

        // Assert
        await act.Should()
            .ThrowAsync<DomainEventContextCancelledException>();
    }

    private EventMediator CreateEventMediator(Type[] types)
    {
        var serviceProvider = new ServiceCollection()
           .AddApplicationHandlersFromTypes(types)
           .BuildServiceProvider();

        var eventHandlerRegistryService = serviceProvider.GetRequiredService<IEventHandlerRegistryService>();
        var loggerMock = new Mock<ILogger<EventMediator>>().Object;

        return new EventMediator(serviceProvider,
            eventHandlerRegistryService,
            loggerMock);
    }

    public class MockDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }
    }

    public class MockDomainEventHandler : IDomainEventHandler<MockDomainEvent>
    {
        public Task HandleAsync(MockDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }

    public class MockDomainEvent2Handler : IDomainEventHandler<MockDomainEvent>
    {
        public Task HandleAsync(MockDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }

    public class MockDomainEventPreProcessorHandler : IPreProcessorHandler<MockDomainEvent>
    {
        public Task PreProcessAsync(
            IDomainEventData<MockDomainEvent> eventData,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class MockDomainEventPreProcessorCanceledHandler : IPreProcessorHandler<MockDomainEvent>
    {
        public Task PreProcessAsync(
            IDomainEventData<MockDomainEvent> eventData,
            CancellationToken cancellationToken = default)
        {
            eventData.Cancel(code: "Cancelled", message: "Cancelled");
            return Task.CompletedTask;
        }
    }

    public class MockEntity : EntityBase
    {

    }

    public class MockEntityCreateEventHandler : IEntityCreatedEventHandler<MockEntity>
    {

        public Task HandleAsync(IEntityCreatedEvent<MockEntity> domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
