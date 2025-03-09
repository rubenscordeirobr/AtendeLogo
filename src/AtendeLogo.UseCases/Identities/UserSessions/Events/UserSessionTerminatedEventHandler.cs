using AtendeLogo.Domain.Entities.Identities.Events;

namespace AtendeLogo.UseCases.Identities.UserSessions.Events;

internal class UserSessionTerminatedEventHandler : IDomainEventHandler<UserSessionTerminatedEvent>
{
    private readonly ILogger<UserSessionTerminatedEventHandler> _logger;

    public UserSessionTerminatedEventHandler(ILogger<UserSessionTerminatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(
        UserSessionTerminatedEvent domainEvent )
    {
        //TODO: disconnect user session
        _logger.LogInformation(
            "User session {UserSessionId} terminated. Reason: {Reason}",
            domainEvent.UserSession.Id,
            domainEvent.Reason);

        return Task.CompletedTask;
    }
}
