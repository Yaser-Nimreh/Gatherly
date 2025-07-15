using Domain.Abstractions;

namespace Domain.Primitives;

public class SoftDeletableEntity<TId>(TId id) : AuditableEntity<TId>(id), ISoftDeletableEntity where TId : notnull, IEquatable<TId>
{
    public bool IsDeleted { get; private set; } = false;
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedById { get; private set; }
    public string? DeletedByName { get; private set; }

    public void Delete(Guid? deletedById = null, string? deletedByName = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedById = deletedById;
        DeletedByName = deletedByName;
    }

    public void UnDelete()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedById = null;
        DeletedByName = null;
    }
}

public abstract class SoftDeletableEntity(Guid id) : SoftDeletableEntity<Guid>(id), ISoftDeletableEntity;