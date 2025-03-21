namespace AtendeLogo.Shared.Enums;

public enum UserStatus
{
    [UndefinedValue]
    Unknown = 0,
    Anonymous = 1,
    New,
    System,
    Online,
    Offline,
    Away,
    Busy,
    DoNotDisturb
}
