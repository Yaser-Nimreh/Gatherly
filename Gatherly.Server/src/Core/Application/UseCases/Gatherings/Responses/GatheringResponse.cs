using Application.UseCases.Attendee.Responses;
using Application.UseCases.Invitations.Responses;
using Application.UseCases.Members.Responses;
using Domain.Enums;

namespace Application.UseCases.Gatherings.Responses;

public sealed record GatheringResponse(
    Guid Id,
    string Name,
    DateTime ScheduledAt,
    string? Location,
    GatheringType Type,
    int? MaximumNumberOfAttendees,
    DateTime? InvitationsExpireAt,
    int NumberOfAttendees,
    MemberResponse? Creator,
    IReadOnlyCollection<InvitationResponse> Invitations,
    IReadOnlyCollection<AttendeeResponse> Attendees);