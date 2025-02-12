namespace AtendeLogo.Shared.Enums;

public enum SessionTerminationReason
{
    IpAddressChanged,
    UserAgentChanged,
    PasswordChanged,
    SessionTimeout,
    DomainEventError,
}
