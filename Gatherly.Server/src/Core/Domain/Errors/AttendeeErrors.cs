using Domain.Results;

namespace Domain.Errors;

public static class AttendeeErrors
{
    public static readonly Error AlreadyAttending = Error.Conflict(
        "Attendees.AlreadyAttending",
        "The member is already attending this gathering.");

    public static Error NotFound(Guid attendeeId) => Error.NotFound(
        "Attendees.NotFound",
        $"The attendee with Id = '{attendeeId}' was not found.");
}