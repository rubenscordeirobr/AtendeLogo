namespace AtendeLogo.Shared.Enums;

public enum UserRole
{
    [UndefinedValue]
    None = 0,
    Anonymous,
    SystemAdmin,
    Owner,
    Admin,
    Operator,
    Viewer,
    ChatAgent,
}
