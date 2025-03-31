namespace AtendeLogo.Shared.Enums;

public enum ActivityType
{
    [UndefinedValue]
    Undefined = 0,
    Created,
    Updated,
    Deleted,
    Read,
    Authenticated,
    UserLoginSuccess,
    UserLoginFailed,
    UserLogout,
}
