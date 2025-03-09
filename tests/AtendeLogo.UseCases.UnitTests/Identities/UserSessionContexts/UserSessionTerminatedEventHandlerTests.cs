using AtendeLogo.Common.Infos;
using AtendeLogo.Domain.Entities.Identities.Events;
using AtendeLogo.Domain.Entities.Identities.Factories;
using AtendeLogo.UseCases.Identities.UserSessions.Events;

namespace AtendeLogo.UseCases.UnitTests.Identities.UserSessionContexts;

public class UserSessionTerminatedEventHandlerTests
     : IClassFixture<AnonymousServiceProviderMock>
{
    private Fixture _fixture = new();
    private IEventMediator _eventMediator;

    public UserSessionTerminatedEventHandlerTests(AnonymousServiceProviderMock serviceProviderMock)
    {
        _eventMediator = serviceProviderMock.GetRequiredService<IEventMediator>();
    }

    [Fact]
    public async Task EventMediator_ShouldDispatchForUserSessionTerminatedEvent()
    {
        // Arrange
        var user = _fixture.Create<TenantUser>();
        var userSession = UserSessionFactory.Create(
            user: user,
            clientHeaderInfo: ClientRequestHeaderInfo.System,
            authenticationType: AuthenticationType.System,
            rememberMe: false,
            tenant_id: null);

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

