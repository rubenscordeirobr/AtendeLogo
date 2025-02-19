using AtendeLogo.Application.Models.Identities;
using AtendeLogo.Common.Infos;

namespace AtendeLogo.Application.Factories;

public static class UserSessionFactory
{
    public static IUserSession CreateAnonymousSession(
        RequestHeaderInfo headerInfo)
    {
        return new AnonymousUserSession
        {
            ApplicationName = headerInfo.ApplicationName,
            IpAddress = headerInfo.IpAddress,
            UserAgent = headerInfo.UserAgent
        };
    }
}
