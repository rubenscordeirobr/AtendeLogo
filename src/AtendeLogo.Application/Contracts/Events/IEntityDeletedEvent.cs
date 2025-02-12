﻿using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.Application.Contracts.Events;

public interface IEntityDeletedEvent<TEntity> 
    : IEntityStateChangedEvent<TEntity>
    where TEntity : EntityBase
{
    IReadOnlyList<IPropertyValueEvent> PropertyValues { get; }
}

