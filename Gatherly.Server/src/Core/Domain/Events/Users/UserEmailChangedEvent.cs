using Domain.Primitives;

namespace Domain.Events.Users;

public sealed record UserEmailChangedEvent(Guid Id, Guid UserId) : DomainEvent(Id);