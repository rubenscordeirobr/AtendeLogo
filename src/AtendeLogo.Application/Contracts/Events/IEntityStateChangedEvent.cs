using AtendeLogo.Domain.Enums;
using AtendeLogo.Domain.Primitives;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Contracts.Events;

public interface IEntityStateChangedEvent : IDomainEvent
{
    EntityChangeState State { get; }

    EntityBase EntityBase { get; }
}

public interface IEntityStateChangedEvent<TEntity> : IEntityStateChangedEvent
    where TEntity : EntityBase
{
    TEntity Entity { get; }

    EntityBase IEntityStateChangedEvent.EntityBase
        => Entity;
}

