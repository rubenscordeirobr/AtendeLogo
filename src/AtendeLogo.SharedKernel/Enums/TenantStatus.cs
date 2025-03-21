namespace AtendeLogo.Shared.Enums;

public enum TenantStatus
{
    [UndefinedValue]
    Unknown = 0,
    Active,
    Inactive,
    Suspended,
    Pending,
    Archived
}
