namespace AtendeLogo.Domain.Primitives.Abstractions;

public interface IAggregateRoot
{

}

public interface IEventAggregate : IAggregateRoot
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
}
