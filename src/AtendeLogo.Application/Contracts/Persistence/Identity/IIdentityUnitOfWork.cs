namespace AtendeLogo.Application.Contracts.Persistence.Identity;

public interface IIdentityUnitOfWork: IUnitOfWork
{
    IUserSessionRepository UserSessionRepository { get; }
    IAdminUserRepository AdminUserRepository { get; }

    ISystemUserRepository SystemUserRepository { get; }

    ITenantUserRepository TenantUserRepository { get; }

    ITenantRepository TenantRepository { get; }
   
}
