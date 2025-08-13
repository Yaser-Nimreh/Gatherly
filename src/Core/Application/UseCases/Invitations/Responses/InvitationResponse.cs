using Domain.Enums;

namespace Application.UseCases.Invitations.Responses;

public sealed record InvitationResponse(
    Guid Id,
    Guid GatheringId,
    Guid MemberId,
    InvitationStatus Status);