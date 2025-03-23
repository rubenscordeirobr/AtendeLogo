namespace AtendeLogo.Application.Contracts.Events;

public interface IEntityCreatedEvent<TEntity>
    : IEntityStateChangedEvent<TEntity>
    where TEntity : EntityBase
{
    IReadOnlyList<IPropertyValueEvent> PropertyValues { get; }

    EntityChangeState IEntityStateChangedEvent.State
        => EntityChangeState.Created;
}

