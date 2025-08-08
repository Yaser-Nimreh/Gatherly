using Domain.Primitives;

namespace Domain.Events.Members;

public sealed record MemberEmailChangedEvent(Guid Id, Guid MemberId) : DomainEvent(Id);