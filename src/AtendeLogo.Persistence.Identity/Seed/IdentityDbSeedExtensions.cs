﻿using AtendeLogo.Common.Infos;
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
        var anonymousSession_Id = AnonymousIdentityConstants.AnonymousSystemSession_Id;

        var anonymousUser = new SystemUser(
            name: "Anonymous",
            email: "anonymous@atendelogo.com.br",
            language: Language.Default,
            role: UserRole.None,
            userState: UserState.Active,
            userStatus: UserStatus.Anonymous,
            phoneNumber: phoneNumber,
            password);

        anonymousUser.SetAnonymousId();
         
        var headerInfo = new ClientRequestHeaderInfo(
            IpAddress: "LOCALHOST",
            ApplicationName: "AtendeLogo.Seed",
            UserAgent: "SYSTEM"
        );

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
        var phoneNumber = PhoneNumber.Create("+5542998373996").GetValue();

        var password = Password.Create(strongPassword, "SYSTEM").GetValue();
        var anonymousSession_Id = AnonymousIdentityConstants.AnonymousSystemSession_Id;

        var anonymousUser = new AdminUser(
            name: "Super Admin",
            email: "superadmin@atendelogo.com.br",
            language: Language.Default,
            role: UserRole.Admin,
            userState: UserState.Active,
            userStatus: UserStatus.Anonymous,
            phoneNumber: phoneNumber,
            password);

        anonymousUser.SetCreateSession(AnonymousIdentityConstants.AnonymousSystemSession_Id);
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
            email: SystemTenantConstants.TenantSystemEmail,
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
             language: Language.Default,
             userState: UserState.Active,
             userStatus: UserStatus.System,
             role: UserRole.Admin,
             phoneNumber: phoneNumber,
             password: password
         );

        systemTenant.SetCreateSession(AnonymousIdentityConstants.AnonymousSystemSession_Id);
        tenantUser.SetCreateSession(AnonymousIdentityConstants.AnonymousSystemSession_Id);

        tenantUser.SetPropertyValue(x => x.Id, SystemTenantConstants.TenantSystemOwnerUser_Id);
        systemTenant.SetPropertyValue(x => x.Id, SystemTenantConstants.TenantSystem_Id);

        dbContext.Add(systemTenant);
    }
}
