namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public record UpdateDefaultTenantAddressCommand : CommandRequest<SuccessResponse>
{
    public Guid Tenant_Id { get; init; }
    public required string AddressName { get; init; }
    public required AddressDto Address { get; init; }
}
