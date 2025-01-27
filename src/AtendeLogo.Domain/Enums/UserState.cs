namespace AtendeLogo.Domain.Enums;

public enum UserState
{
    New = 0,
    Active,
    Inactive ,
    Suspended,
    Deleted,
    PendingVerification,
    Blocked
}