namespace AtendeLogo.Shared.Enums;

public enum SessionTerminationReason
{
    IpAddressChanged = 1,
    UserAgentChanged,
    PasswordChanged,
    SessionExpired,
    DomainEventError,
}
