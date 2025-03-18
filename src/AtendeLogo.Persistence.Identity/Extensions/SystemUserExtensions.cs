namespace AtendeLogo.Persistence.Identity.Extensions;

public static class SystemUserExtensions
{
    public static void SetAnonymousId(this SystemUser user)
    {
        if (user.Id != default)
        {
            throw new InvalidOperationException("Id is already set.");
        }

        user.SetCreateSession(AnonymousIdentityConstants.Session_Id);
        user.SetPropertyValue(p => p.Id, AnonymousIdentityConstants.User_Id);
        user.SetPropertyValue(p => p.Email, AnonymousIdentityConstants.Email);
        user.SetPropertyValue(p => p.Name, AnonymousIdentityConstants.Name);
        user.SetPropertyValue(p => p.UserStatus, UserStatus.Anonymous);
        user.SetPropertyValue(p => p.UserState, UserState.Active);
    }
}
