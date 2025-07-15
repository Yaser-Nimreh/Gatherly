using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class AuditableEntity<TId>(TId id) : Entity<TId>(id), IAuditableEntity where TId : notnull, IEquatable<TId>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public string ItemType => GetType().Name;
}

public abstract class AuditableEntity(Guid id) : AuditableEntity<Guid>(id), IAuditableEntity;