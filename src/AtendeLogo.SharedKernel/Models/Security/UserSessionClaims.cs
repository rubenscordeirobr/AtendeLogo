namespace AtendeLogo.Shared.Models.Security;

public record UserSessionClaims(
    string Name,
    string Email,
    string PhoneNumber,
    Guid Session_Id,
    Language Language,
    UserRole UserRole,
    UserType UserType,
    DateTime? Expiration = null
);
