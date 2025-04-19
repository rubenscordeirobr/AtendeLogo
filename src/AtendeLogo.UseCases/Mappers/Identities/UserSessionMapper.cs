namespace AtendeLogo.UseCases.Mappers.Identities;

internal static class UserSessionMapper
{
    internal static UserSessionResponse ToResponse(UserSession userSession)
    {
        Guard.NotNull(userSession);

        return new UserSessionResponse
        {
            Id = userSession.Id,
            ApplicationName = userSession.ApplicationName,
            IpAddress = userSession.IpAddress,
            UserAgent = userSession.UserAgent,
            IsActive = userSession.IsActive,
            KeepSession = userSession.KeepSession,
            StartedAt = userSession.StartedAt,
            TerminatedAt = userSession.TerminatedAt,
            LastActivity = userSession.LastActivity,
            Culture = userSession.Culture,
            AuthenticationType = userSession.AuthenticationType,
            TerminationReason = userSession.TerminationReason,
            UserRole = userSession.UserRole,
            UserType = userSession.UserType,
            GeoLocation = userSession.GeoLocation,
            Tenant_Id = userSession.Tenant_Id,
            User_Id = userSession.User_Id
        };
    }
}
