using AtendeLogo.Domain.Enums;
using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.Application.Contracts.Events;
 
public interface IEntityUpdatedEvent<TEntity> : IEntityStateChangedEvent<TEntity>
    where TEntity : EntityBase
{
    IReadOnlyList<IChangedPropertyEvent> ChangedProperties { get; }

    EntityChangeState IEntityStateChangedEvent.State
        => EntityChangeState.Updated;
}
