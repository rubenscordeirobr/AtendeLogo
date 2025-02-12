namespace AtendeLogo.Shared.Interfaces.Identities;

public interface ITenantOwned
{
    Guid Tenant_Id { get; }
}
