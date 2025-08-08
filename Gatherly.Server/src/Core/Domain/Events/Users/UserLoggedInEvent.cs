using Domain.Primitives;

namespace Domain.Events.Users;

public sealed record UserLoggedInEvent(Guid Id, Guid UserId) : DomainEvent(Id);