using AtendeLogo.Persistence.Common.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace AtendeLogo.Persistence.Common.Configurations;

public static class EnumConfiguration
{
    private static readonly Dictionary<Type, HashSet<Type>> _enumTypeMappings = new();

    public static NpgsqlDbContextOptionsBuilder AddMapEnum<TContext, TEnum>(
        this NpgsqlDbContextOptionsBuilder optionsBuilder)
         where TContext : DbContext
         where TEnum : struct, Enum
    {
        optionsBuilder.MapEnum<TEnum>();

#if DEBUG
        EnumMappingsTracker<TContext>.AddEnumType<TEnum>();
#endif
        return optionsBuilder;
    }
    
    public static void CheckEnums<TContext>(
       this IMutableProperty mutableProperty)
       where TContext : DbContext

    {
#if DEBUG
        if (mutableProperty.ClrType.IsEnum)
        {
            if (!EnumMappingsTracker<TContext>.Contains(mutableProperty.ClrType))
            {
                var declaredTypeName = mutableProperty.PropertyInfo?.DeclaringType?.GetQualifiedName() ?? "Unknown";
                var errorMessages = $"The enum type '{mutableProperty.ClrType?.Name}' not mapped." +
                                    $" The property '{declaredTypeName}.{mutableProperty.Name}' must map the enum in NpgsqlDbContextOptionsBuilder using AddMapEnum method";

                throw new EnumTypeNotMappedException(errorMessages);
            }
        }
#endif
    }
 
#if DEBUG
    private class EnumMappingsTracker<TContext> 
        where TContext : DbContext
    {
        private static readonly Type _dbContextType = typeof(TContext);
        private static HashSet<Type> _enumTypesMapped
            => _enumTypeMappings.GetOrAdd(_dbContextType, () => new HashSet<Type>());

        public static void AddEnumType<TEnum>()
            where TEnum : Enum
        {
            _enumTypesMapped.Add(typeof(TEnum));
        }

        internal static bool Contains<TEnum>()
             where TEnum : Enum
        {
            return _enumTypesMapped.Contains(typeof(TEnum));
        }

        internal static bool Contains(Type enumType)
        {
            return _enumTypesMapped.Contains(enumType);
        }
    }
#endif
}
