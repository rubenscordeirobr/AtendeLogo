namespace AtendeLogo.Domain.Entities.Identities;

public sealed class Tenant : EntityBase, ITenant, ISoftDeletableEntity, IEventAggregate
{
    private readonly List<IDomainEvent> _events = new();
    private readonly List<TenantUser> _users = new();
    private readonly List<TenantAddress> _addresses = new();

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

    public Guid? OwnerUser_Id { get; private set; }
    public TenantUser? OwnerUser { get; private set; }

    public Guid? Address_Id { get; private set; }
    public TenantAddress? DefaultAddress { get; private set; }

    public IReadOnlyList<UserSession> Sessions { get; private set; } = new List<UserSession>();
    public IReadOnlyList<TenantUser> Users => _users;
    public IReadOnlyList<TenantAddress> Addresses => _addresses;

    // EF Core constructor
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

    public void SetDefaultAddress(TenantAddress address)
    {
        DefaultAddress?.RemoveDefault();

        DefaultAddress = address;
        Address_Id = address.Id;
    }

    public void SetCreateOwner(TenantUser tenantUser)
    {
        if(OwnerUser_Id != default)
        {
            throw new InvalidOperationException(" The owner cannot be changed.");
        }

        Guard.NotEmpty(tenantUser.Id);
        OwnerUser = tenantUser;
        OwnerUser_Id = tenantUser.Id;

        _events.Add(new TenantCreatedEvent(this, tenantUser));
    }

    public void SetTimeZoneOffset(TimeZoneOffset timeZoneOffset)
    {
        if (!TimeZoneOffset.Equals(timeZoneOffset))
            return;

        var previousOffset = TimeZoneOffset;

        TimeZoneOffset = timeZoneOffset;

        var changedEvent = new TenantTimeZoneOffsetChangedEvent(
            Tenant: this,
            PreviousTimeZoneOffset: previousOffset,
            TimeZoneOffset: timeZoneOffset);

        _events.Add(changedEvent);
    }

    public TenantAddress AddAddress(
        string addressName,
        string street,
        string number,
        string? complement,
        string neighborhood,
        string city,
        string state,
        string zipCode,
        Country country)
    {
        var address = new TenantAddress(
             tenant: this,
             addressName: addressName,
             street: street,
             number: number,
             complement: complement,
             neighborhood: neighborhood,
             city: city,
             state: state,
             zipCode: zipCode,
             country: country
         );

        _addresses.Add(address);

        _events.Add(new TenantAddressAddedEvent(this, address));
        return address;
    }

    public void SetAddressDefault(TenantAddress address)
    {
        if (DefaultAddress == null && Address_Id != default)
        {
            throw new InvalidOperationException("The address must be loaded before updating it.");
        }

        if (DefaultAddress != null)
        {
            DefaultAddress.RemoveDefault();
        }

        var previousAddress= DefaultAddress;
        

        DefaultAddress = address;
        DefaultAddress.SetDefault();

        _events.Add(new TenantDefaultAddressUpdatedEvent(this, previousAddress, address));
    }

    public TenantUser AddUser(
        string name,
        string email,
        UserState userState,
        UserStatus userStatus,
        TenantUserRole tenantUserRole,
        PhoneNumber phoneNumber,
        Password password)
    {
        var user = new TenantUser(
             tenant: this,
             name: name,
             email: email,
             userState: userState,
             userStatus: userStatus,
             tenantUserRole: tenantUserRole,
             phoneNumber: phoneNumber,
             password: password
         );

        _users.Add(user);
        _events.Add(new TenantUserAddedEvent(this, user));
        return user;
    }

    #region IEntityDeleted, IOrderableEntity, IDomainEventAggregate
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedSession_Id { get; private set; }

    IReadOnlyList<IDomainEvent> IEventAggregate.DomainEvents
        => _events;

    #endregion
}
