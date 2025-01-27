using AtendeLogo.Domain.Domain;

namespace AtendeLogo.Domain.Entities.Identities;

public class Tenant : EntityBase, ITenant
{
    public string Name { get; private set; }
    public string FiscalCode { get; private set; }
    public string ContactEmail { get; private set; }
    public string ContactPhone { get; private set; }
    public string Country { get; private set; }
    public string DefaultLanguage { get; private set; }
    public string DefaultCurrency { get; private set; }
    public string PhoneNumber { get; private set; }
    public Address? Address { get; private set; }
    public Guid AddressId { get; private set; }
    public BusinessType BusinessType { get; private set; }
    public TenantState TenantState { get; private set; }
    public TentantStatus TenantStatus { get; private set; }
    public TenantType TenantType { get; private set; }

    public Tenant(
        string name,
        string fiscalCode,
        string contactEmail,
        string contactPhone,
        string country,
        string defaultLanguage,
        string defaultCurrency,
        string phoneNumber,
        BusinessType businessType,
        TenantState tenantState,
        TentantStatus tenantStatus,
        TenantType tenantType)
    {
        Name = name;
        FiscalCode = fiscalCode;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Country = country;
        DefaultLanguage = defaultLanguage;
        DefaultCurrency = defaultCurrency;
        PhoneNumber = phoneNumber;
        BusinessType = businessType;
        TenantState = tenantState;
        TenantStatus = tenantStatus;
        TenantType = tenantType;
    }

    public void SetAddress(Address address)
    {
        Address = address;
        AddressId = address.Id;
    }

    IAddress? ITenant.Address => Address;
}
