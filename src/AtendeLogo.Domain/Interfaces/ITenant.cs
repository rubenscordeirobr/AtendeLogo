namespace AtendeLogo.Domain.Entities.Identities;
public interface ITenant
{
    string Name { get; }
    string FiscalCode { get; }
    string ContactEmail { get; }
    string ContactPhone { get; }
    string Country { get; }
    string DefaultLanguage { get; }
    string DefaultCurrency { get; }
    string PhoneNumber { get; }
    IAddress? Address { get; }
    BusinessType BusinessType { get; }
    TenantState TenantState { get; }
    TentantStatus TenantStatus { get; }
    TenantType TenantType { get; }
}
