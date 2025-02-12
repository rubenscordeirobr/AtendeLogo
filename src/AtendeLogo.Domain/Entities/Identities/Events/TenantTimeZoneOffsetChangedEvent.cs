namespace AtendeLogo.Domain.Entities.Identities.Events;

internal sealed record TenantTimeZoneOffsetChangedEvent(
    Tenant Tenant,
    TimeZoneOffset TimeZoneOffsetOld,
    TimeZoneOffset TimeZoneOffsetNew) : IDomainEvent;

