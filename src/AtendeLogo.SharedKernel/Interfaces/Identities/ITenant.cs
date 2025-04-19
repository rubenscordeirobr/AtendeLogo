namespace AtendeLogo.Shared.Interfaces.Identities;
public interface ITenant
{
    string Name { get; }
    string Email { get; }
    Country Country { get; }
    Culture Culture { get; }
    Currency Currency { get; }
    BusinessType BusinessType { get; }
    TenantType TenantType { get; }
    FiscalCode FiscalCode { get; }
    PhoneNumber PhoneNumber { get; }
    TimeZoneOffset TimeZoneOffset { get; }
}
