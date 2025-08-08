using Domain.Primitives;

namespace Domain.Events.Users;

public sealed record UserNameChangedEvent(Guid Id, Guid UserId) : DomainEvent(Id);