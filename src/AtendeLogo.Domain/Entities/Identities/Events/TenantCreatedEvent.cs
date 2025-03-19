namespace AtendeLogo.Domain.Entities.Identities.Events;

public sealed record TenantCreatedEvent(
        Tenant Tenant,
        TenantUser Owner) : IDomainEvent;

public sealed record TenantUpdatedEvent(
        Tenant Tenant) : IDomainEvent;
