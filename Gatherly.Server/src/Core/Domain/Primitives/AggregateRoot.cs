using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class AggregateRoot<TId>(TId id) : SoftDeletableEntity<TId>(id), IAggregateRoot where TId : notnull, IEquatable<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}

public abstract class AggregateRoot(Guid id) : AggregateRoot<Guid>(id), IAggregateRoot;