using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AtendeLogo.Persistence.Common.Interceptors;

public class DefaultSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        // Add your logic before saving changes
        throw new Exception("Use SaveChangesAsync instead");
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        throw new Exception("Use SaveChangesAsync instead");
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            var entries = eventData.Context.ChangeTracker.Entries<EntityBase>()
                .Where(entry => entry.State == EntityState.Added ||
                           entry.State == EntityState.Modified);
           

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entity.CreatedSession_Id == default ||
                   entity.LastUpdatedSession_Id == default)
                {
                    throw new InvalidOperationException("" +
                        "The SaveChanges method must be call into the UnitOfWork class");
                }

                if (!Debugger.IsAttached)
                {
                    if (entity.LastUpdatedAt.AddSeconds(10) < DateTime.UtcNow)
                    {
                        var message = $"The entity {entity.GetType().Name} {entity.Id} " +
                                      $" The last update must e less than 10 seconds";

                        throw new InvalidOperationException(message);
                    }
                }
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
     
    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        //logic after saving changes asynchronously
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
     
    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        //logic when saving changes fails asynchronously
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }
}
