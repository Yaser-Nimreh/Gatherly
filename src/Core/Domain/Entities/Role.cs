using Domain.Abstractions;
using Domain.Events.Roles;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class Role : IdentityRole<Guid>, IAggregateRoot, ISoftDeletableEntity, IAuditableEntity, IEntity
{
    public Role(Guid id, string name, string description, bool isSystemRole)
    {
        Id = id;
        Name = name;
        NormalizedName = name.ToUpperInvariant();
        Description = description;
        IsSystemRole = isSystemRole;
    }

    private Role() : base() { }

    public string Description { get; private set; } = string.Empty;
    public bool IsSystemRole { get; private set; }

    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<User> Users { get; set; } = [];

    public static Role Create(Guid id, string name, string description, bool isSystemRole = false)
    {
        var role = new Role(id, name, description, isSystemRole);

        role.Raise(new RoleCreatedEvent(Guid.NewGuid(), role.Id));

        return role;
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public Guid? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public string ItemType => GetType().Name;

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

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}