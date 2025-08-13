using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class AggregateRoot : SoftDeletableEntity, IAggregateRoot
{
    protected AggregateRoot(Guid id) : base(id) { }

    protected AggregateRoot() { }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}