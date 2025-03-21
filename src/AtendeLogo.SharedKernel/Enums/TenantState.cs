namespace AtendeLogo.Shared.Enums;

public enum TenantState
{
    [UndefinedValue]
    Unknown = 0,
    New,
    Onboarding,
    Trial,
    Operational,
    Cancelled,
    Closed,
    System
}
