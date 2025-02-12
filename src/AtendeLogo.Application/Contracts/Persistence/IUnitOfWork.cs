using AtendeLogo.Application.Common;
using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.Application.Contracts.Persistence;

public interface IUnitOfWork : IAsyncDisposable
{
    //transaction
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<SaveChangesResult> CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(Exception exception, CancellationToken cancellationToken = default);

    //commands
    public void Add<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public void Update<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public void Delete<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public void Attach<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    Task<SaveChangesResult> SaveChangesAsync(CancellationToken cancellationToken = default);
}
