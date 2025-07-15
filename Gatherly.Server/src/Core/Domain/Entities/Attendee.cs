using Domain.Abstractions;

namespace Domain.Entities;

public sealed class Attendee : IAuditableEntity, ISoftDeletableEntity
{
    internal Attendee(Invitation invitation) : this()
    {
        GatheringId = invitation.GatheringId;
        MemberId = invitation.MemberId;
    }

    private Attendee() { }

    public Guid GatheringId { get; private set; }
    public Guid MemberId { get; private set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public string ItemType => GetType().Name;

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