using System.Diagnostics.CodeAnalysis;
using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Abstractions;

public interface IClientTenantUserSessionContext : IClientUserSessionContext
{

    [MemberNotNullWhen(true, nameof(Tenant))]
    new bool IsAuthenticated { get; }

    TenantResponse? Tenant { get; }

    void SetSessionContext(
        UserSessionClaims userSessionClaims,
        UserSessionResponse userSession,
        UserResponse user,
        TenantResponse tenant);

}
