﻿using AtendeLogo.Application.Contracts.Persistence.Activities;
using AtendeLogo.Domain.Entities.Activities;
using AtendeLogo.Domain.Entities.Identities.Events;

namespace AtendeLogo.UseCases.Activities.Events;

public sealed class UserLoggedOutEventHandler : IDomainEventHandler<UserLoggedOutEvent>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IHttpContextSessionAccessor _userSessionAccessor;
    private readonly ILogger<UserLoggedOutEventHandler> _logger;

    public UserLoggedOutEventHandler(
        IActivityRepository activityRepository,
        IHttpContextSessionAccessor userSessionAccessor,
        ILogger<UserLoggedOutEventHandler> logger)
    {
        _activityRepository = activityRepository;
        _userSessionAccessor = userSessionAccessor;
        _logger = logger;
    }

    public async Task HandleAsync(UserLoggedOutEvent domainEvent)
    {
        Guard.NotNull(domainEvent);

        var userSession = _userSessionAccessor.GetRequiredUserSession();
        var user = domainEvent.User;

        var description = $"User {user.Name} logged out.";

        var activity = new UserLogoutActivity
        {
            Tenant_Id = userSession.Tenant_Id,
            UserSession_Id = userSession.Id,
            ActivityDate = DateTime.UtcNow,
            Description = description,
            IpAddress = domainEvent.IpAddress,
            User_Id = user.Id
        };

        try
        {
            await _activityRepository.AddAsync(activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to add activity [{ActivityType}] for user type [{UserType}] with ID [{UserId}]. " +
                "Tenant ID: {TenantId}, Session ID: {SessionId}, IP: {IpAddress}",
                nameof(UserLogoutActivity),
                user.GetType().Name,
                user.Id,
                userSession.Tenant_Id,
                userSession.Id,
                domainEvent.IpAddress);
            throw;
        }
    }
}
