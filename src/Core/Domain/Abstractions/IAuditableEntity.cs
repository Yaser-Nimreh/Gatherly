namespace Domain.Abstractions;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    Guid? CreatedById { get; set; }
    string? CreatedByName { get; set; }
    DateTime? LastUpdatedAt { get; set; }
    Guid? LastUpdatedById { get; set; }
    string? LastUpdatedByName { get; set; }
    string ItemType { get; }
}