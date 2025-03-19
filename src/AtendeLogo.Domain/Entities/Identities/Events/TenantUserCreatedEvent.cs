namespace AtendeLogo.Domain.Entities.Identities.Events;

public record TenantUserCreatedEvent(
    Tenant Tenant,
    TenantUser User ) : IDomainEvent;

public record TenantUserRemovedEvent(
    Tenant Tenant,
    TenantUser User) : IDomainEvent;

