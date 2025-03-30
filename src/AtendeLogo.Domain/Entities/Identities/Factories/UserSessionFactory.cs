using AtendeLogo.Common.Infos;

namespace AtendeLogo.Domain.Entities.Identities.Factories;

public static class UserSessionFactory
{
    public static UserSession Create(
        IUser user,
        ClientRequestHeaderInfo clientHeaderInfo,
        AuthenticationType authenticationType,
        bool keepSession,
        Guid? tenant_id)
    {
        Guard.NotNull(user);
        Guard.NotNull(clientHeaderInfo);
        Guard.NotEmpty(user.Id);

        Guard.EnumNotDefined(user.UserType);
        Guard.EnumNotDefined(user.Role);

        var newSession = new UserSession(
             applicationName: clientHeaderInfo.ApplicationName,
             ipAddress: clientHeaderInfo.IpAddress,
             userAgent: clientHeaderInfo.UserAgent,
             isActive: true,
             keepSession: keepSession,
             authenticationType: authenticationType,
             language: user.Language,
             userRole: user.Role,
             userType: user.UserType,
             user_Id: user.Id,
             tenant_Id: tenant_id
        );
        newSession.AddSessionStartedEvents(user);
        return newSession;
    }
}
