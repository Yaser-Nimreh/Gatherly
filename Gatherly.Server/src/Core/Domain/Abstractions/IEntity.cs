namespace Domain.Abstractions;

public interface IEntity<TId> where TId : notnull, IEquatable<TId>
{
    TId Id { get; }
}

public interface IEntity : IEntity<Guid>;