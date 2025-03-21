using AtendeLogo.Common.Infos;
using AtendeLogo.Domain.Entities.Identities.Factories;

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

        if (dbContext is not IDbSeedAsync _)
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
        var strongPassword = "%ANONYMOUS@anonymous%";
        var phoneNumber = PhoneNumber.Create("+5542999999999").GetRequiredValue();
        var password = Password.Create(strongPassword, "ANONYMOUS").GetRequiredValue();

        var anonymousUser = new SystemUser(
            name: "Anonymous",
            email: "anonymous@atendelogo.com.br",
            language: Language.Default,
            role: UserRole.Anonymous,
            userState: UserState.Active,
            userStatus: UserStatus.Anonymous,
            phoneNumber: phoneNumber,
            password);

        anonymousUser.SetAnonymousId();

        var headerInfo = ClientRequestHeaderInfo.System;

        var anonymousUserSession = UserSessionFactory.Create(
            user: anonymousUser,
            clientHeaderInfo: headerInfo,
            authenticationType: AuthenticationType.Anonymous,
            rememberMe: true,
            tenant_id: null);

        anonymousUserSession.SetAnonymousSystemSessionId();

        dbContext.Add(anonymousUser);
        dbContext.Add(anonymousUserSession);
    }

    private static void AddSuperAdminUser(this IdentityDbContext dbContext)
    {
        var strongPassword = "SuperAdmin@Teste%#";
        var phoneNumber = PhoneNumber.Create("+5542998373996").GetRequiredValue();

        var password = Password.Create(strongPassword, "SYSTEM").GetRequiredValue();

        var anonymousUser = new AdminUser(
            name: SuperAdminUserConstants.Name,
            email: SuperAdminUserConstants.Email,
            language: Language.Default,
            role: UserRole.Admin,
            userState: UserState.Active,
            userStatus: UserStatus.Anonymous,
            phoneNumber: phoneNumber,
            password);

        anonymousUser.SetCreateSession(AnonymousIdentityConstants.Session_Id);
        dbContext.Add(anonymousUser);
    }

    internal static void AddSystemTenant(this IdentityDbContext dbContext)
    {
        var strongPassword = "TenantAdmin@Teste%#";
        var password = Password.Create(strongPassword, "SYSTEM")
            .GetRequiredValue();

        var phoneNumber = PhoneNumber.Create(SystemTenantConstants.PhoneNumber)
            .GetRequiredValue();

        var fiscalCode = FiscalCode.Create(SystemTenantConstants.FiscalCode, Country.Brazil).Value!;

        var systemTenant = new Tenant(
            name: SystemTenantConstants.Name,
            email: SystemTenantConstants.Email,
            businessType: BusinessType.System,
            country: Country.Brazil,
            currency: Currency.BRL,
            language: Language.English,
            tenantState: TenantState.System,
            tenantStatus: TenantStatus.Active,
            tenantType: TenantType.System,
            fiscalCode: fiscalCode,
            phoneNumber: phoneNumber,
            timeZoneOffset: TimeZoneOffset.Default
        );

        var tenantUser = systemTenant.CreateUser(
             name: SystemTenantConstants.Name,
             email: SystemTenantConstants.Email,
             language: Language.Default,
             userState: UserState.Active,
             userStatus: UserStatus.System,
             role: UserRole.Admin,
             phoneNumber: phoneNumber,
             password: password
         );

        systemTenant.SetCreateSession(AnonymousIdentityConstants.Session_Id);
        tenantUser.SetCreateSession(AnonymousIdentityConstants.Session_Id);

        tenantUser.SetPropertyValue(x => x.Id, SystemTenantConstants.OwnerUser_Id);
        systemTenant.SetPropertyValue(x => x.Id, SystemTenantConstants.Tenant_Id);

        dbContext.Add(systemTenant);
    }
}
