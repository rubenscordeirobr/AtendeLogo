namespace AtendeLogo.Domain.Primitives.Interfaces;

public interface IEntityTenant
{
    Guid? TenantId { get; }
}
