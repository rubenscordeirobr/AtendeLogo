using AtendeLogo.Common;
using AtendeLogo.Shared.ValueObjects;
using AtendeLogo.Persistence.Identity.Extensions;

namespace AtendeLogo.Persistence.Identity.Seed;

internal static class IdentityDbSeedExtensions
{
    private static readonly SemaphoreSlim _lock = new(1, 1);

    internal static async Task SeedAsync(this IdentityDbContext dbContext)
    {
        await _lock.WaitAsync();
        try
        {
            await dbContext.SeedAsyncInternal();
        }
        finally
        {
            _lock.Release();
        }
    }
    private static async Task SeedAsyncInternal(this IdentityDbContext dbContext)
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        dbContext.AddAnonymousUserAndSessionAsync();
        dbContext.AddSuperAdminUser();
        dbContext.AddSystemTenant();

        if (!(dbContext is IDbSeedAsync dbSeed))
        {
            throw new InvalidOperationException("DbContext must implement IDbSeedAsync");
        }

        var result = await (dbContext as IDbSeedAsync).SeedSaveChangesAsync();
        if (result < 1)
        {
            throw new InvalidOperationException("Seed failed");
        }
    }

    private static void AddAnonymousUserAndSessionAsync(this IdentityDbContext dbContext)
    {
        var strongPassword = "%ANONYMOUS@anymous%";
        var phoneNumber = PhoneNumber.Create("+5542999999999").GetValue();
        var password = Password.Create(strongPassword, "ANONYMOUS").GetValue();
        var anonymousSession_Id = AnonymousConstants.AnonymousSystemSession_Id;
        var anonymousUser = new SystemUser(
            name: "Anonymous",
            email: "anonymous@atendelogo.com.br",
            userState: UserState.Active,
            userStatus: UserStatus.Anonymous,
            phoneNumber: phoneNumber,
            password);

        
        anonymousUser.SetAnonymousId();

        var clientToken = HashHelper.CreateSha256Hash(AnonymousConstants.AnonymousSystemSession_Id);

        Guard.Sha256(clientToken);

        var anonymousUserSession = new UserSession(
            applicationName: "AtendeLogo.Seed",
            clientSessionToken: clientToken,
            ipAddress: "LOCALHOST",
            authToken: null,
            userAgent: "SYSTEM",
            language: Language.Default,
            authenticationType: AuthenticationType.Anonymous,
            user_Id: AnonymousConstants.AnonymousUser_Id,
            tenant_Id: null
        );
        anonymousUserSession.SetAnonymousSystemSessionId();

        dbContext.Add(anonymousUser);
        dbContext.Add(anonymousUserSession);
    }

    private static void AddSuperAdminUser(this IdentityDbContext dbContext)
    {
        var strongPassword = "SuperAdmin@Teste%#";
        var phoneNumber = PhoneNumber.Create("+5542998373996").GetValue();

        var password = Password.Create(strongPassword, "ANONYMOUS").GetValue();
        var anonymousSession_Id = AnonymousConstants.AnonymousSystemSession_Id;

        var anonymousUser = new AdminUser(
            name: "Super Admin",
            email: "superadmin@atendelogo.com.br",
            userState: UserState.Active,
            userStatus: UserStatus.Anonymous,
            adminUserRole: AdminUserRole.SuperAdmin,
            phoneNumber: phoneNumber,
            password);

        anonymousUser.SetCreateSession(AnonymousConstants.AnonymousSystemSession_Id);
        dbContext.Add(anonymousUser);
    }

    internal static void AddSystemTenant(this IdentityDbContext dbContext)
    {
        var strongPassword = "TenantAdmin@Teste%#";
        var password = Password.Create(strongPassword, "SYSTEM")
            .GetValue();

        var phoneNumber = PhoneNumber.Create(SystemTenantConstants.PhoneNumber)
            .GetValue();

        var systemTenant = new Tenant(
            name: SystemTenantConstants.TenantSystemName,
            fiscalCode: SystemTenantConstants.FiscalCode,
            email: SystemTenantConstants.TenantSystemName,
            businessType: BusinessType.System,
            country: Country.Brazil,
            currency: Currency.BRL,
            language: Language.English,
            tenantState: TenantState.System,
            tenantStatus: TenantStatus.Active,
            tenantType: TenantType.System,
            phoneNumber: phoneNumber,
            timeZoneOffset: TimeZoneOffset.Default
        );

        var tenantUser = systemTenant.AddUser(
             name: SystemTenantConstants.TenantSystemName,
             email: SystemTenantConstants.TenantSystemEmail,
             userState: UserState.Active,
             userStatus: UserStatus.System,
             tenantUserRole: TenantUserRole.Admin,
             phoneNumber: phoneNumber,
             password: password
         );

        systemTenant.SetCreateSession(AnonymousConstants.AnonymousSystemSession_Id);
        tenantUser.SetCreateSession(AnonymousConstants.AnonymousSystemSession_Id);
        
        tenantUser.SetPropertyValue(x => x.Id, SystemTenantConstants.TenantSystemOwnerUser_Id);
        systemTenant.SetPropertyValue(x => x.Id, SystemTenantConstants.TenantSystem_Id);

        dbContext.Add(systemTenant);

        
    }
}
