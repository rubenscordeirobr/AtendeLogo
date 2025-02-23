namespace AtendeLogo.Persistence.Identity.Extensions;

public static class SystemUserExtensions
{
    public static void SetAnonymousId(this SystemUser user)
    {
        if (user.Id != default)
        {
            throw new InvalidOperationException("Id is already set.");
        }

        user.SetCreateSession(AnonymousConstants.AnonymousSystemSession_Id);
        user.SetPropertyValue(p => p.Id, AnonymousConstants.AnonymousUser_Id);
        user.SetPropertyValue(p => p.Email, AnonymousConstants.AnonymousEmail);
        user.SetPropertyValue(p => p.Name, AnonymousConstants.AnonymousName);
        user.SetPropertyValue(p => p.UserStatus, UserStatus.Anonymous);
        user.SetPropertyValue(p => p.UserState, UserState.Active);
    }
}
