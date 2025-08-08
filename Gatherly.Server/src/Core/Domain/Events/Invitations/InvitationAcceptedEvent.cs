using Domain.Primitives;

namespace Domain.Events.Invitations;

public sealed record InvitationAcceptedEvent(Guid Id, Guid InvitationId, Guid GatheringId) : DomainEvent(Id);