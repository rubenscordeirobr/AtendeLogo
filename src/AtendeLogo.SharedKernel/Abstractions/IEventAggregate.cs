namespace AtendeLogo.Shared.Abstractions;

public interface IAggregateRoot
{

}

public interface IEventAggregate : IAggregateRoot
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
}
