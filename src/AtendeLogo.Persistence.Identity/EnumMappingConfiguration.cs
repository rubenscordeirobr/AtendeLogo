using AtendeLogo.Persistence.Common.Configurations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace AtendeLogo.Persistence.Identity;

public static partial class EnumMappingConfiguration
{
    public static void ConfigureEnumMappings<TContext>(
        this NpgsqlDbContextOptionsBuilder optionsBuilder)
        where TContext : DbContext 
    {
        optionsBuilder
            .AddMapEnum<TContext, UserStatus>()
            .AddMapEnum<TContext, AdminUserRole>()
            .AddMapEnum<TContext, AuthenticationType>()
            .AddMapEnum<TContext, BusinessType>()
            .AddMapEnum<TContext, Country>()
            .AddMapEnum<TContext, Currency>()
            .AddMapEnum<TContext, Language>()
            .AddMapEnum<TContext, PasswordStrength>()
            .AddMapEnum<TContext, TenantState>()
            .AddMapEnum<TContext, TenantStatus>()
            .AddMapEnum<TContext, TenantType>()
            .AddMapEnum<TContext, TenantUserRole>()
            .AddMapEnum<TContext, UserState>();
    }
}
