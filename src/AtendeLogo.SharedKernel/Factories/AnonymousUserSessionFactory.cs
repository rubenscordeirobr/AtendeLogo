using AtendeLogo.Common.Infos;
using AtendeLogo.Shared.Interfaces.Identities;
using AtendeLogo.Shared.Models.Identities;

namespace AtendeLogo.Shared.Factories;

public static class AnonymousUserSessionFactory
{
    public static IUserSession CreateAnonymousSession( ClientRequestHeaderInfo headerInfo)
    {
        return new AnonymousUserSession
        {
            ApplicationName = headerInfo.ApplicationName,
            IpAddress = headerInfo.IpAddress,
            UserAgent = headerInfo.UserAgent
        };
    }
}
