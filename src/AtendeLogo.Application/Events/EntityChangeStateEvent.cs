using AtendeLogo.Domain.Enums;

namespace AtendeLogo.Application.Events;

public abstract record EntityChangeStateEvent<TEntity>: IEntityStateChangedEvent<TEntity>
    where TEntity : EntityBase
{
    public TEntity Entity { get; }
    public abstract EntityChangeState State { get; }
    
    protected EntityChangeStateEvent(TEntity entity)
    {
        Entity = entity;
    }
}
