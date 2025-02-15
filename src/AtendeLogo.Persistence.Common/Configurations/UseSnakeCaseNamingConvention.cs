using AtendeLogo.Common.Utils;

namespace AtendeLogo.Persistence.Common.Configurations;

public static class UseSnakeCaseNamingConventionExtensions
{
    public static void UseSnakeCaseNamingConvention(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = CaseUtils.ToSnakeCase(entity.GetTableName() ?? string.Empty);
            entity.SetTableName(tableName);

            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    property.SetColumnName(CaseUtils.ToSnakeCase(columnName));
                }
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (!string.IsNullOrWhiteSpace(indexName))
                {
                    index.SetDatabaseName(CaseUtils.ToSnakeCase(indexName));
                }
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    key.SetName(CaseUtils.ToSnakeCase(keyName));
                }
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var foreignKeyName = foreignKey.GetConstraintName();
                if (!string.IsNullOrWhiteSpace(foreignKeyName))
                {
                    foreignKey.SetConstraintName(CaseUtils.ToSnakeCase(foreignKeyName));
                }
            }
        }
    }

}
