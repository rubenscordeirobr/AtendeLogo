namespace AtendeLogo.Application.Contracts.Events;

public interface IEntityStateChangedEventHandler<TEvent> : IDomainEventHandler<TEvent>
    where TEvent : IEntityStateChangedEvent
{
}

public interface IEntityCreatedEventHandler<TEntity>
    : IEntityStateChangedEventHandler<IEntityCreatedEvent<TEntity>>
    where TEntity : EntityBase
{
}

public interface IEntityUpdatedEventHandler<TEntity>
    : IEntityStateChangedEventHandler<IEntityUpdatedEvent<TEntity>>
    where TEntity : EntityBase
{

}

public interface IEntityDeletedEventHandler<TEntity>
    : IEntityStateChangedEventHandler<IEntityDeletedEvent<TEntity>>
    where TEntity : EntityBase
{

}
