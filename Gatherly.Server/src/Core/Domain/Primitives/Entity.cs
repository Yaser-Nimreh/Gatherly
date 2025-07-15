using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class Entity<TId>(TId id) : IEntity<TId>, IEquatable<Entity<TId>> where TId : notnull, IEquatable<TId>
{
    public TId Id { get; private init; } = id;

    public static bool operator ==(Entity<TId>? first, Entity<TId>? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity<TId>? first, Entity<TId>? second) =>
        !(first == second);

    public bool Equals(Entity<TId>? other)
    {
        if (other is null || other.GetType() != GetType())
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj is Entity<TId> entity &&
               EqualityComparer<TId>.Default.Equals(Id, entity.Id);
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}

public abstract class Entity(Guid id) : Entity<Guid>(id), IEntity;