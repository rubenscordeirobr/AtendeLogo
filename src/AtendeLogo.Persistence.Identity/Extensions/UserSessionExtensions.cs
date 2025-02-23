namespace AtendeLogo.Persistence.Identity.Extensions;

internal static class UserSessionExtensions
{
    internal static void SetAnonymousSystemSessionId(this UserSession session)
    {
        if (session.Id != default)
        {
            throw new InvalidOperationException("Id is already set.");
        }

        session.SetCreateSession(AnonymousConstants.AnonymousSystemSession_Id);
        session.SetPropertyValue(p => p.Id, AnonymousConstants.AnonymousSystemSession_Id);
        session.SetPropertyValue(p => p.AuthenticationType, AuthenticationType.Anonymous);
        session.SetPropertyValue(p => p.AuthToken, null);
        session.SetPropertyValue(p => p.Tenant_Id, null);
    }
}
