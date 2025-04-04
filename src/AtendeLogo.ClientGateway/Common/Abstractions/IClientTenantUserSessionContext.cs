using System.Diagnostics.CodeAnalysis;
using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IClientTenantUserSessionContext
{

    [MemberNotNullWhen(true, nameof(UserSessionClaims))]
    [MemberNotNullWhen(true, nameof(UserSession))]
    [MemberNotNullWhen(true, nameof(User))]
    [MemberNotNullWhen(true, nameof(Tenant))]

    bool IsAuthenticated { get; }

    UserSessionClaims? UserSessionClaims { get; }

    UserSessionResponse? UserSession { get; }

    UserResponse? User { get; }

    TenantResponse? Tenant { get; }
     
    void SetSessionContext(
        UserSessionClaims userSessionClaims,
        UserSessionResponse userSession,
        UserResponse user, 
        TenantResponse tenant );

    void ClearSessionContext();
}
