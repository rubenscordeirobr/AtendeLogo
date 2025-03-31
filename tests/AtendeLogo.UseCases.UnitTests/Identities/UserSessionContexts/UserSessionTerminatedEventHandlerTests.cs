using AtendeLogo.Common.Infos;
using AtendeLogo.Domain.Entities.Identities.Events;
using AtendeLogo.Domain.Entities.Identities.Factories;
using AtendeLogo.UseCases.Identities.UserSessions.Events;

namespace AtendeLogo.UseCases.UnitTests.Identities.UserSessionContexts;

public class UserSessionTerminatedEventHandlerTests
     : IClassFixture<ServiceProviderMock<AnonymousRole>>
{
    private readonly Fixture _fixture = new();
    private readonly IEventMediator _eventMediator;

    public UserSessionTerminatedEventHandlerTests(
        ServiceProviderMock<AnonymousRole> serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _eventMediator = serviceProviderMock.GetRequiredService<IEventMediator>();
    }

    [Fact]
    public async Task EventMediator_ShouldDispatchForUserSessionTerminatedEvent()
    {
        // Arrange
        var user = SystemTenantConstants.OwnerUser; ;
        var session = AnonymousUserConstants.AnonymousUserSession;

        var userSession = UserSessionFactory.Create(
            user: user,
            clientHeaderInfo: ClientRequestHeaderInfo.System,
            authenticationType: AuthenticationType.System,
            keepSession: false,
            tenant_id: null);

        var createdEvent = new UserSessionTerminatedEvent(userSession, SessionTerminationReason.SessionExpired);
        var eventContext = new DomainEventContext(session, [createdEvent]);

        // Act
        await _eventMediator.DispatchAsync(eventContext);
        // Assert

        eventContext
            .ShouldHaveExecutedEvent(createdEvent)
            .WithHandler<UserSessionTerminatedEventHandler>();
    }
}

