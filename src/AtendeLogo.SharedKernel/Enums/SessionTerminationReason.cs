namespace AtendeLogo.Shared.Enums;

public enum SessionTerminationReason
{
    [UndefinedValue]
    Unknown = 0,
    IpAddressChanged,
    UserAgentChanged,
    PasswordChanged,
    SessionExpired,
    DomainEventError,
    UserLogout,
}
