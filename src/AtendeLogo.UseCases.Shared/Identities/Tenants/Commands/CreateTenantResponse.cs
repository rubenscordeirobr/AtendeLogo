using AtendeLogo.UseCases.Common;

namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public sealed record CreateTenantResponse(Guid tenant_Id) : ResponseBase
{

}
