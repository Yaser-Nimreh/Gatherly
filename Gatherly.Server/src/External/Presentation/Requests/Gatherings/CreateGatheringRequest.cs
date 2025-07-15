using Domain.Enums;

namespace Presentation.Requests.Gatherings;

public sealed record CreateGatheringRequest(
    Guid MemberId,
    GatheringType Type,
    DateTime ScheduledAt,
    string Name,
    string? Location,
    int? MaximumNumberOfAttendees,
    int? InvitationsValidBeforeInHours);