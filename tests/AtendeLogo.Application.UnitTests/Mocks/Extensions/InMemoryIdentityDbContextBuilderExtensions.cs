using AtendeLogo.Application.UnitTests.Mocks.EFCore;
using AtendeLogo.Application.UnitTests.Mocks.Extensions;
using AtendeLogo.Persistence.Identity;
using AtendeLogo.Persistence.Identity.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AtendeLogo.Application.UnitTests.Mocks.Extensions;

public static class InMemoryIdentityDbContextBuilderExtensions
{
    public static IServiceCollection AddInMemoryIdentityDbContext(
           this IServiceCollection services)
    {
        services
            .AddSingleton<IModelCustomizer, InMemoryIdentityDbContextModelCustomizer>()
            .AddDbContext<IdentityDbContext>(
                optionsBuilder =>
                {
                    optionsBuilder.UseInMemoryDatabase(
                        databaseName: "ShouldReturnSystemUser",
                        optionsBuilder =>
                        {
                            optionsBuilder.ConfigureEnumMappings<IdentityDbContext>();
                        });
                    optionsBuilder.ReplaceService<IModelCustomizer, InMemoryIdentityDbContextModelCustomizer>();
                },
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Singleton);
         

        using (var serviceProvider = services.BuildServiceProvider())
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                context.Database.EnsureCreated();
                context.SeedAsync().Wait();
            }
        }

        return services;
    }

    public static DbContextOptionsBuilder UseInMemoryDatabase(
        this DbContextOptionsBuilder optionsBuilder,
        string databaseName,
        Action<RelationalInMemoryDbContextOptionsBuilder> inMemoryOptionsBuilderAction)
    {
        lock (optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName);
            var inMemoryOptions = new RelationalInMemoryDbContextOptionsBuilder(optionsBuilder);
            inMemoryOptionsBuilderAction?.Invoke(inMemoryOptions);
        }
        return optionsBuilder;
    }
}
