﻿using AtendeLogo.Common;
using AtendeLogo.Shared.ValueObjects;

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

        if (!(dbContext is IDbSeedAsync dbSeed))
        {
            throw new InvalidOperationException("DbContext must implement IDbSeedAsync");
        }

        var result = await (dbContext as IDbSeedAsync).SeedSaveChangesAsync();
        if (result < 2)
        {
            throw new InvalidOperationException("Seed failed");
        }
    }

}
