﻿namespace AtendeLogo.Application.Contracts.Persistence.Identities;

public interface IIdentityUnitOfWork: IUnitOfWork
{
    IUserSessionRepository UserSessions { get; }
    IAdminUserRepository AdminUsers { get; }
    ISystemUserRepository SystemUsers { get; }

    ITenantUserRepository TenantUsers { get; }

    ITenantRepository Tenants { get; }
 
}
