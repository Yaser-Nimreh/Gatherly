using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class AuditableEntity : Entity, IAuditableEntity
{
    protected AuditableEntity(Guid id) : base(id) { }

    protected AuditableEntity() { }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public string ItemType => GetType().Name;
}