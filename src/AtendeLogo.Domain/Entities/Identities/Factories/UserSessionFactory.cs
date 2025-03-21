using AtendeLogo.Common.Helpers;
using AtendeLogo.Common.Infos;

namespace AtendeLogo.Domain.Entities.Identities.Factories;

public static class UserSessionFactory
{
    public static UserSession Create(
        IUser user,
        ClientRequestHeaderInfo clientHeaderInfo,
        AuthenticationType authenticationType,
        bool rememberMe,
        Guid? tenant_id )
    {
        Guard.NotNull(user);
        Guard.NotNull(clientHeaderInfo);

        var clientSessionToken = HashHelper.CreateSha256Hash(Guid.NewGuid());
        var expirationTime = rememberMe
            ? TimeSpan.FromDays(365)
            : TimeSpan.FromMinutes(30);

        return new UserSession(
             applicationName: clientHeaderInfo.ApplicationName,
             clientSessionToken: clientSessionToken,
             ipAddress: clientHeaderInfo.IpAddress,
             userAgent: clientHeaderInfo.UserAgent,
             authenticationType: authenticationType,
             userRole: user.Role,
             userType: user.UserType,
             language: user.Language,
             expirationTime: expirationTime,
             user_Id: user.Id,
             tenant_Id: tenant_id
        );
    }
}
