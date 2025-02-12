namespace AtendeLogo.Domain.Entities.Identities.Events;

public sealed record TenantCreatedEvent(
        Tenant Tenant,
        TenantUser Owner) : IDomainEvent;

