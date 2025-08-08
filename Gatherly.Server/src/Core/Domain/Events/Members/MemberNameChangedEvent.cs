using Domain.Primitives;

namespace Domain.Events.Members;

public sealed record MemberNameChangedEvent(Guid Id, Guid MemberId) : DomainEvent(Id);