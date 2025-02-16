using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Persistence.Common.Interceptors;
using AtendeLogo.Persistence.Identity.Repositories;
using AtendeLogo.Persistence.Identity.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.Persistence.Identity;

public static class IdentityPersistenceServiceConfiguration
{
    public static IServiceCollection AddIdentityPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddNpgsqlIdentityDbContext(configuration)
            .AddIdentyRepositoryServices();
    }

    private static IServiceCollection AddNpgsqlIdentityDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityPostgresql");

        return services.AddDbContext<IdentityDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connectionString, npgOptionsBuilder =>
            {
                npgOptionsBuilder.ConfigureEnumMappings<IdentityDbContext>();
            });

            optionsBuilder.AddInterceptors(new DefaultValuesInterceptor())
                .AddInterceptors(new DefaultSaveChangesInterceptor());

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif

        }, 
        contextLifetime: ServiceLifetime.Scoped,
        optionsLifetime: ServiceLifetime.Singleton
        );
    }

    internal static IServiceCollection AddIdentyRepositoryServices(this IServiceCollection services)
    {
        return services.AddScoped<IAdminUserRepository, AdminUserRepository>()
             .AddScoped<ITenantUserRepository, TenantUserRepository>()
             .AddScoped<ISystemUserRepository, SystemUserRepository>()
             .AddScoped<ITenantRepository, TenantRepository>()
             .AddScoped<IUserSessionRepository, UserSessionRepository>()
             .AddTransient<IIdentityUnitOfWork, IdentityUnitOfWork>();
    }

    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<IdentityDbContext>();

        dbContext.Database.Migrate();
        await dbContext.SeedAsync();
    }
}
