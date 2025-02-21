namespace AtendeLogo.Domain.Entities.Identities.Events;

internal sealed record TenantTimeZoneOffsetChangedEvent(
    Tenant Tenant,
    TimeZoneOffset PreviousTimeZoneOffset,
    TimeZoneOffset TimeZoneOffset) : IDomainEvent;

