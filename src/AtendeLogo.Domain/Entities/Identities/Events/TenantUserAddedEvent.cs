namespace AtendeLogo.Domain.Entities.Identities.Events;

public record TenantUserAddedEvent(
    Tenant Tenant,
    TenantUser User ) : IDomainEvent;

public record TenantUserRemovedEvent(
    Tenant Tenant,
    TenantUser User) : IDomainEvent;

