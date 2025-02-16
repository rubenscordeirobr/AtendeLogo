﻿using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.UseCases.Activities.Events;

public sealed class EntityUpdatedPreProcessorHandler<TEntity>
    : IEntityUpdatedEventPreProcessorHandler<TEntity>
    where TEntity : EntityBase
{
    public Task PreProcessAsync(
        IDomainEventData<IEntityUpdatedEvent<TEntity>> eventData, 
        CancellationToken cancellationToken = default)
    {
        //Just for testing purposes
        return Task.CompletedTask;
    }
}
