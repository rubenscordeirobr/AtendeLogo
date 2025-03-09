using System.Linq.Expressions;

namespace AtendeLogo.Application.Contracts.Persistence;
public interface IRepositoryBase<TEntity> where TEntity : EntityBase
{
    //queries
    Task<TEntity?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);
   
    Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
   
    Task<IEnumerable<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default);
  
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);

    Task<TEntity> RefreshAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    Task<TEntity?> TryRefreshAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default);
}
