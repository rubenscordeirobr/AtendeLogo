namespace AtendeLogo.Shared.Enums;

public enum UserState
{
    [UndefinedValue]
    Unknown = 0,
    New = 1,
    Active,
    Inactive ,
    Suspended,
    Deleted,
    PendingVerification,
    Blocked
}
