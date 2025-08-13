using Domain.Primitives;

namespace Domain.Events.Members;

public sealed record MemberRegisteredEvent(Guid Id, Guid MemberId) : DomainEvent(Id);