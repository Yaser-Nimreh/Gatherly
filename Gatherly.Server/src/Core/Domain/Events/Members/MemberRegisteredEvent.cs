using Domain.Abstractions;

namespace Domain.Events.Members;

public sealed record MemberRegisteredEvent(Guid MemberId) : IDomainEvent;