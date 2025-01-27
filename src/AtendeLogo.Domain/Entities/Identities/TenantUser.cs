
using AtendeLogo.Domain.Domain.Interfaces;

namespace AtendeLogo.Domain.Entities.Identities;

public class TenantUser : User, IOrderableEntity
{
    public Guid TenantId { get; private set; }
    public Tenant? Tenant { get; private set; }
    public TenantUserRole TenantUserRole { get; private set; }
    public IList<TenantUser> TenantUsers { get; } = new List<TenantUser>();
    public double? Order { get; private set; }
  
    public TenantUser(
        string name,
        string email,
        string phoneNumber,
        Password password,
        UserState userState,
        UserStatus userStatus,
        Guid tenantId,
        double? order = null
        ) :
        base(name, email, phoneNumber, password, userState, userStatus)
    {
        TenantId = tenantId;

        if (order is not null)
        {
            Order = order;
        }
    }
}