using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class Entity : IEntity, IEquatable<Entity>
{
    protected Entity(Guid id) => Id = id;

    protected Entity() { }

    public Guid Id { get; private init; }

    public static bool operator ==(Entity? first, Entity? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity? first, Entity? second) =>
        !(first == second);

    public bool Equals(Entity? other)
    {
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return obj is Entity entity && entity.Id == Id;
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}