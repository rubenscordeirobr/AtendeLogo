using AtendeLogo.Domain.Entities.Shared;

namespace AtendeLogo.Domain.Entities.Identities;

public sealed class Tenant : EntityBase, ITenant, ISoftDeletableEntity, IEventAggregate
{
    private readonly List<IDomainEvent> _events = new();

    public string Name { get; private set; }
    public string FiscalCode { get; private set; }
    public string Email { get; private set; }
    public BusinessType BusinessType { get; private set; }
    public Country Country { get; private set; }
    public Currency Currency { get; private set; }
    public Language Language { get; private set; }
    public TenantState TenantState { get; private set; }
    public TenantStatus TenantStatus { get; private set; }
    public TenantType TenantType { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    public TimeZoneOffset TimeZoneOffset { get; private set; }

    public Guid? Owner_Id { get; private set; }
    public TenantUser? Owner { get; private set; }

    public Guid? Address_Id { get; private set; }
    public Address? Address { get; private set; }

    public List<TenantUser> Users { get; private set; } = [];
    public List<UserSession> Sessions { get; private set; } = [];

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private Tenant(
        string name,
        string fiscalCode,
        string email,
        BusinessType businessType,
        Country country,
        Currency currency,
        Language language,
        TenantState tenantState,
        TenantStatus tenantStatus,
        TenantType tenantType,
        PhoneNumber phoneNumber)
        : this(name, fiscalCode, email, businessType, country, currency,
              language, tenantState, tenantStatus, tenantType, phoneNumber, TimeZoneOffset.Default)
    {

    }

    public Tenant(
        string name,
        string fiscalCode,
        string email,
        BusinessType businessType,
        Country country,
        Currency currency,
        Language language,
        TenantState tenantState,
        TenantStatus tenantStatus,
        TenantType tenantType,
        PhoneNumber phoneNumber,
        TimeZoneOffset timeZoneOffset)
    {
        Name = name;
        Email = email;
        FiscalCode = fiscalCode;
        BusinessType = businessType;
        Country = country;
        Currency = currency;
        Language = language;
        TenantState = tenantState;
        TenantStatus = tenantStatus;
        TenantType = tenantType;
        PhoneNumber = phoneNumber;
        TimeZoneOffset = timeZoneOffset;
    }

    public void SetAddress(Address address)
    {
        Address = address;
        Address_Id = address.Id;
    }

    public void SetCreateOwner(TenantUser tenantUser)
    {
        Guard.MustBeEmpty(Owner_Id);
        Guard.NotEmpty(tenantUser.Id);

        Owner = tenantUser;
        Owner_Id = tenantUser.Id;

        _events.Add(new TenantCreatedEvent(this, tenantUser));
    }

    public void SetTimeZoneOffset(TimeZoneOffset timeZoneOffset)
    {
        if (!TimeZoneOffset.Equals(timeZoneOffset))
            return;

        var previousOffset = TimeZoneOffset;
        TimeZoneOffset = timeZoneOffset;

        var @event = new TenantTimeZoneOffsetChangedEvent(
            Tenant: this,
            TimeZoneOffsetOld: previousOffset,
            TimeZoneOffsetNew: timeZoneOffset);

        _events.Add(@event);
    }

    #region IEntityDeleted, IOrderableEntity, IDomainEventAggregate
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedSession_Id { get; private set; }

    IReadOnlyList<IDomainEvent> IEventAggregate.DomainEvents
        => _events;

    #endregion
}
