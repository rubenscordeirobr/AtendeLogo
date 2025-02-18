using AtendeLogo.Application.UnitTests.Mocks.EFCore;
using AtendeLogo.Common;
using AtendeLogo.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtendeLogo.Application.UnitTests.Mocks.Extensions;

public static class InMemoryEntityBuilderConfiguration
{
    public static ModelBuilder ConfigureInMemoryEntities(
      this ModelBuilder modelBuilder)
    {
        var entitiesType = modelBuilder.Model.GetEntityTypes();
        foreach (var mutableEntityType in entitiesType)
        {
            var entityType = mutableEntityType.ClrType;
            if (!entityType.IsSubclassOf<EntityBase>())
            {
                continue;
            }
            var entityBuilder = modelBuilder.Entity(entityType);
            entityBuilder.ConfigureInMemoryEntity();
        }
        return modelBuilder;
    }

    public static EntityTypeBuilder ConfigureInMemoryEntity(
        this EntityTypeBuilder entityBuilder)
    {
        var entityType = entityBuilder.Metadata.ClrType;
        Guard.NotNull(entityType);

        if (!entityType.IsSubclassOfOrEquals<EntityBase>())
        {
            throw new InvalidOperationException($"The entity {entityType.Name} must be a subclass of EntityBase");
        }

        var baseType = entityType.BaseType;

        var idProperty = entityBuilder.Property(nameof(EntityBase.Id));

        Guard.NotNull(idProperty);

        idProperty
            .HasValueGenerator<GuidIntegerValueGenerator>()
            .ValueGeneratedOnAdd();
            
        return entityBuilder;
    }
}
