using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class SoftDeletableEntity : AuditableEntity, ISoftDeletableEntity
{
    protected SoftDeletableEntity(Guid id) : base(id) { }

    protected SoftDeletableEntity() { }

    public bool IsDeleted { get; private set; }
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