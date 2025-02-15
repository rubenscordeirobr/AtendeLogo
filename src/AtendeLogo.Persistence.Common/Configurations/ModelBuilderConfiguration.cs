using AtendeLogo.Persistence.Common.Exceptions;

namespace AtendeLogo.Persistence.Common.Configurations;

public static partial class ModelBuilderConfiguration
{
    public static void ConfigureModelDefaultConfiguration<TContext>(
        this ModelBuilder modelBuilder)
        where TContext : DbContext
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.HasCollation("case_accent_insensitive",
            locale: "und-u-ks-level1",
            provider: "icu",
            deterministic: false);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TContext).Assembly)
            .UseCollation("case_accent_insensitive");

        modelBuilder.UseSnakeCaseNamingConvention();

        var entitiesType = modelBuilder.Model.GetEntityTypes();

        foreach (var mutableEntityType in entitiesType)
        {
            var entityType = mutableEntityType.ClrType;
            if (!entityType.IsSubclassOf<EntityBase>())
            {
                continue;
            }

            var entityBuilder = modelBuilder.Entity(entityType);
            if (entityBuilder == null)
            {
                throw new Exception($"Entity builder not found for {mutableEntityType.ClrType.Name}");
            }

            entityBuilder.ConfigureEntityDefaultSettings();

            mutableEntityType
                .ConfigureValueObjects(entityBuilder)
                .ConfigureProperties();

            mutableEntityType.Validate<TContext>();
        }

       

        // Reapply naming convention to ensure consistency after potential index or configuration changes
        modelBuilder.UseSnakeCaseNamingConvention();
    }
}
