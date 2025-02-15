using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Persistence.Common.Enums;
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
        IUserSessionService userSessionService,
        IEventMediator eventMediator,
        ILogger<IIdentityUnitOfWork> logger,
        TrackingOption trackingOption = TrackingOption.Tracking)
        : base(dbContext, userSessionService, eventMediator, logger)
    {
        _trackingOption = trackingOption;
    }

    public IAdminUserRepository AdminUserRepository
        => LazyInitialize(
            ref _adminUserRepository,
            () => new AdminUserRepository(DbContext, UserSessionService, _trackingOption));

    public ISystemUserRepository SystemUserRepository
        => LazyInitialize(
            ref _systemUserRepository,
            () => new SystemUserRepository(DbContext, UserSessionService, _trackingOption));

    public ITenantUserRepository TenantUserRepository
        => LazyInitialize(
            ref _tenantUserTenantRepository,
            () => new TenantUserRepository(DbContext, UserSessionService, _trackingOption));

    public ITenantRepository TenantRepository
        => LazyInitialize(
            ref _tenantRepository,
            () => new TenantRepository(DbContext, UserSessionService, _trackingOption));

    public IUserSessionRepository UserSessionRepository
        => LazyInitialize(
            ref _userSessionRepository,
            () => new UserSessionRepository(DbContext, UserSessionService, _trackingOption));

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
