namespace AtendeLogo.Persistence.Identity.Extensions;

internal static class UserSessionExtensions
{
    internal static void SetAnonymousSystemSessionId(this UserSession session)
    {
        if (session.Id != default)
        {
            throw new InvalidOperationException("Id is already set.");
        }
        
        var clientToken = HashHelper.CreateSha256Hash(AnonymousIdentityConstants.Session_Id);

        session.SetCreateSession(AnonymousIdentityConstants.Session_Id);
        session.SetPropertyValue(p => p.Id, AnonymousIdentityConstants.Session_Id);
        session.SetPropertyValue(p => p.AuthenticationType, AuthenticationType.Anonymous);
        session.SetPropertyValue(p => p.AuthToken, null);
        session.SetPropertyValue(p => p.Tenant_Id, null);
        session.SetPropertyValue(x => x.ClientSessionToken, clientToken);
    }
}
