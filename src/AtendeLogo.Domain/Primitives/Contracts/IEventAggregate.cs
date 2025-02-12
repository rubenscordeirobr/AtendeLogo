namespace AtendeLogo.Domain.Primitives.Contracts;

public interface IAggregateRoot
{

}

public interface IEventAggregate : IAggregateRoot
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
}
