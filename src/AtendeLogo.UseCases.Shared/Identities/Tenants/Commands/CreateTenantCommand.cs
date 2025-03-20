namespace AtendeLogo.UseCases.Identities.Tenants.Commands;
public sealed record CreateTenantCommand : CommandRequest<CreateTenantResponse>
{
    public required string Name { get; init; }
    
    public required string TenantName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required Country Country { get; init; }
    public required Language Language { get; init; }
    public required Currency Currency { get; init; }
    public required BusinessType BusinessType { get; init; }
    public required TenantType TenantType { get; init; }
    public required FiscalCode FiscalCode { get; init; }
    public required PhoneNumber PhoneNumber { get; init; }
}
