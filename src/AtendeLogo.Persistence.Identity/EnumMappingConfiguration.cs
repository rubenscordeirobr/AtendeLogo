using AtendeLogo.Persistence.Common.Configurations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AtendeLogo.Persistence.Identity;

public static partial class EnumMappingConfiguration
{
    public static void ConfigureEnumMappings<TContext>(
        this IRelationalDbContextOptionsBuilderInfrastructure  optionsBuilder)
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
