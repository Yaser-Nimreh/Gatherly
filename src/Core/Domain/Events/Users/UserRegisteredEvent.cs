using Domain.Primitives;

namespace Domain.Events.Users;

public sealed record UserRegisteredEvent(Guid Id, Guid UserId) : DomainEvent(Id);