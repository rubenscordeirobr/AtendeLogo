using AtendeLogo.Persistence.Common.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AtendeLogo.Persistence.Common.Extensions;

public static class EFExtensions
{
    public static IQueryable<TEntity> ApplyTracking<TEntity>(
         this IQueryable<TEntity> dbSet,
         TrackingOption tracking)
         where TEntity : EntityBase
    {
        return tracking == TrackingOption.Tracking
            ? dbSet
            : dbSet.AsNoTracking();
    }

    public static bool HasChanges(this EntityEntry entry)
    {
        return entry.State switch
        {
            EntityState.Added => true,
            EntityState.Modified => true,
            EntityState.Deleted => true,
            _ => false
        };
    }
}

