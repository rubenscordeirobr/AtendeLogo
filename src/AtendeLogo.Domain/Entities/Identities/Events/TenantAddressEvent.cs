namespace AtendeLogo.Domain.Entities.Identities.Events;

public record TenantAddressAddedEvent(
    Tenant tenant,
    TenantAddress newAddress) : IDomainEvent;

public record TenantDefaultAddressUpdatedEvent(
    Tenant tenant,
    TenantAddress? previousAddress,
    TenantAddress address) : IDomainEvent;

public record TenantAddressRemovedEvent(
    Tenant tenant,
    TenantAddress address) : IDomainEvent;
