using AtendeLogo.Domain.Entities.Identities.Events;
using AtendeLogo.UseCases.Identities.UserSessionContexts.Events;

namespace AtendeLogo.UseCases.UnitTests.Identities.UserSessionContexts;

public class UserSessionTerminatedEventHandlerTests
     : IClassFixture<AnonymousServiceProviderMock>
{
    private Fixture _figure = new();
    private IEventMediator _eventMediator;

    public UserSessionTerminatedEventHandlerTests(AnonymousServiceProviderMock serviceProviderMock)
    {
        _eventMediator = serviceProviderMock.GetRequiredService<IEventMediator>();
    }

    [Fact]
    public async Task EventMediator_ShouldDispatchForUserSessionTerminatedEvent()
    {
        // Arrange
        var userSession = _figure.Create<UserSession>();
        var createdEvent = new UserSessionTerminatedEvent(userSession, SessionTerminationReason.SessionExpired);
        var eventContext = new DomainEventContext([createdEvent]);

        // Act
        await _eventMediator.DispatchAsync(eventContext);
        // Assert

        eventContext
            .ShouldHaveExecutedEvent(createdEvent)
            .WithHandler<UserSessionTerminatedEventHandler>();
    }
}

