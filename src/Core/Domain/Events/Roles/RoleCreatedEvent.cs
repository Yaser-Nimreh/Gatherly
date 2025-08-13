using Domain.Primitives;

namespace Domain.Events.Roles;

public sealed record RoleCreatedEvent(Guid Id, Guid RoleId) : DomainEvent(Id);