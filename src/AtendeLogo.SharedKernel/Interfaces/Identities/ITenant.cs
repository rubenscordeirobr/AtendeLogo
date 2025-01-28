using AtendeLogo.Shared.Interfaces.Shared;

namespace AtendeLogo.Shared.Interfaces.Identities;
public interface ITenant
{
    string Name { get; }
    string FiscalCode { get; }
    string ContactEmail { get; }
    string ContactPhone { get; }
    string Country { get; }
    string Language { get; }
    string Currency { get; }
    string PhoneNumber { get; }
    IAddress? Address { get; }
    BusinessType BusinessType { get; }
    TenantState TenantState { get; }
    TenantStatus TenantStatus { get; }
    TenantType TenantType { get; }
}
