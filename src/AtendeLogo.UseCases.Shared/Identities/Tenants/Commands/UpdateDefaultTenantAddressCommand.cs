using AtendeLogo.UseCases.Common;
using AtendeLogo.UseCases.Shared;

namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public record UpdateDefaultTenantAddressCommand : CommandRequest<UpdateDefaultTenantResponse>
{
    public Guid Tenant_Id { get; init; }
    public required string AddressName { get; init; }
    public required AddressDto Address { get; init; }
}

public record UpdateDefaultTenantResponse : ResponseBase
{
} 
