using System.Diagnostics.CodeAnalysis;
using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IClientAdminUserSessionContext
{
    [MemberNotNullWhen(true, nameof(UserSessionClaims))]
    [MemberNotNullWhen(true, nameof(UserSession))]
    [MemberNotNullWhen(true, nameof(User))]

    bool IsAuthenticated { get; }

    UserSessionClaims? UserSessionClaims { get; }

    UserSessionResponse? UserSession { get; }

    UserResponse? User { get; }

    void SetSessionContext(
        UserSessionClaims userSessionClaims,
        UserSessionResponse userSession,
        UserResponse user);

    void ClearSessionContext();
}
