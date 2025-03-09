namespace AtendeLogo.Persistence.Identity.Extensions;

public static class SystemUserExtensions
{
    public static void SetAnonymousId(this SystemUser user)
    {
        if (user.Id != default)
        {
            throw new InvalidOperationException("Id is already set.");
        }

        user.SetCreateSession(AnonymousIdentityConstants.AnonymousSystemSession_Id);
        user.SetPropertyValue(p => p.Id, AnonymousIdentityConstants.AnonymousUser_Id);
        user.SetPropertyValue(p => p.Email, AnonymousIdentityConstants.AnonymousEmail);
        user.SetPropertyValue(p => p.Name, AnonymousIdentityConstants.AnonymousName);
        user.SetPropertyValue(p => p.UserStatus, UserStatus.Anonymous);
        user.SetPropertyValue(p => p.UserState, UserState.Active);
    }
}
