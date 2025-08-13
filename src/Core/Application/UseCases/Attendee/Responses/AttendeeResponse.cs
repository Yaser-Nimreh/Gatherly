namespace Application.UseCases.Attendee.Responses;

public sealed record AttendeeResponse(
    Guid GatheringId,
    Guid MemberId);