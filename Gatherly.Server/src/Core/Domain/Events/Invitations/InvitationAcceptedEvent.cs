using Domain.Abstractions;

namespace Domain.Events.Invitations;

public sealed record InvitationAcceptedEvent(Guid InvitationId, Guid GatheringId) : IDomainEvent;