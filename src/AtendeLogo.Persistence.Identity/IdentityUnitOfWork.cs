using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Persistence.Common.UnitOfWorks;
using AtendeLogo.Persistence.Identity.Repositories;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Persistence.Identity;

internal class IdentityUnitOfWork : UnitOfWork<IdentityDbContext>, IIdentityUnitOfWork
{
    private readonly TrackingOption _trackingOption;

    private IAdminUserRepository? _adminUserRepository;
    private ISystemUserRepository? _systemUserRepository;
    private ITenantUserRepository? _tenantUserTenantRepository;
    private ITenantRepository? _tenantRepository;
    private IUserSessionRepository? _userSessionRepository;

    public IdentityUnitOfWork(
        IdentityDbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        IEventMediator eventMediator,
        ILogger<IIdentityUnitOfWork> logger,
        TrackingOption trackingOption = TrackingOption.Tracking)
        : base(dbContext, userSessionAccessor, eventMediator, logger)
    {
        _trackingOption = trackingOption;
    }

    public IAdminUserRepository AdminUsers
        => LazyInitialize(
            ref _adminUserRepository,
            () => new AdminUserRepository(DbContext, UserSessionAccessor, _trackingOption));

    public ISystemUserRepository SystemUsers
        => LazyInitialize(
            ref _systemUserRepository,
            () => new SystemUserRepository(DbContext, UserSessionAccessor, _trackingOption));

    public ITenantUserRepository TenantUsers
        => LazyInitialize(
            ref _tenantUserTenantRepository,
            () => new TenantUserRepository(DbContext, UserSessionAccessor, _trackingOption));

    public ITenantRepository Tenants
        => LazyInitialize(
            ref _tenantRepository,
            () => new TenantRepository(DbContext, UserSessionAccessor, _trackingOption));

    public IUserSessionRepository UserSessions
        => LazyInitialize(
            ref _userSessionRepository,
            () => new UserSessionRepository(DbContext, UserSessionAccessor, _trackingOption));

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        _adminUserRepository = null;
        _systemUserRepository = null;
        _tenantUserTenantRepository = null;
        _tenantRepository = null;
        _userSessionRepository = null;
    }
}
