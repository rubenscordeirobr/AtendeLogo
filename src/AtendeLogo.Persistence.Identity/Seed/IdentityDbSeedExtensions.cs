using AtendeLogo.Application.Abstractions.Security;
using AtendeLogo.Persistence.Identity.Seeders;

namespace AtendeLogo.Persistence.Identity.Seed;

internal static class IdentityDbSeedExtensions
{
    private static readonly SemaphoreSlim _lock = new(1, 1);

    internal static async Task SeedAsync(
        this IdentityDbContext dbContext,
        ISecureConfiguration secureConfiguration)
    {
        await _lock.WaitAsync();
        try
        {
            if (await dbContext.Users.AnyAsync())
            {
                return;
            }

            var seeder = new IdentityDbSeeder(dbContext, secureConfiguration);
            await seeder.SeedAsync();
        }
        finally
        {
            _lock.Release();
        }
    }
}
