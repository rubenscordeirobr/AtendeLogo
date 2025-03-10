﻿using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Contracts.Events;

public interface IDomainEventData<out TEvent>
    where TEvent : IDomainEvent
{
    IDomainEventContext Context { get; }
    TEvent Event { get; }

    void Cancel(DomainEventError error);

    void Cancel(string code, string message);
}
