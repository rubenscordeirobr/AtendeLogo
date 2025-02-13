using AtendeLogo.Domain.Enums;
using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.Application.Events;

public sealed record EntityUpdatedEventEvent<TEntity> 
    : EntityChangeStateEvent<TEntity>, IEntityUpdatedEvent<TEntity>
    where TEntity : EntityBase
{
    public override EntityChangeState State
        => EntityChangeState.Updated;

    public IReadOnlyList<IChangedPropertyEvent> ChangedProperties { get; }

    public EntityUpdatedEventEvent(
        TEntity entity,
        IReadOnlyList<IChangedPropertyEvent> changedProperties)
        : base(entity)
    {
        ChangedProperties = changedProperties;
    }
}

