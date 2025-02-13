using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Events;

public sealed record ExecutedDomainEventResult(
    IDomainEvent DomainEvent,
    Type HandlerType,
    IApplicationHandler? Handler,
    Exception? Exception)
{
    public bool IsSuccess 
        => Exception is null;
}
