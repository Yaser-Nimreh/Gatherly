using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests.Gatherings;

public sealed record CreateGatheringRequest(
    [Required] Guid MemberId,
    [Required] GatheringType Type,
    [Required] DateTime ScheduledAt,
    [Required] string Name,
    string? Location,
    int? MaximumNumberOfAttendees,
    int? InvitationsValidBeforeInHours);