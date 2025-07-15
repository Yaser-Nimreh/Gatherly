using Domain.Results;

namespace Domain.Errors;

public static class GatheringErrors
{
    public static readonly Error MaximumNumberOfAttendeesIsRequired = Error.Failure(
        "Gatherings.MaximumNumberOfAttendeesIsRequired",
        "Maximum number of attendees is required for gatherings with a fixed number of attendees.");

    public static readonly Error InvitationsValidBeforeInHoursIsRequired = Error.Failure(
        "Gatherings.InvitationsValidBeforeInHoursIsRequired",
        "Invitations valid before in hours is required for gatherings with invitation expiration.");

    public static Error NotFound(Guid gatheringId) => Error.NotFound(
        "Gatherings.NotFound",
        $"The gathering with Id = '{gatheringId}' was not found.");

    public static readonly Error InvitationForCreatorNotAllowed = Error.Problem(
        "Gatherings.InvitationForCreatorNotAllowed",
        "The gathering creator cannot be invited to their own gathering.");

    public static readonly Error AlreadyScheduledInThePast = Error.Problem(
        "Gatherings.AlreadyScheduledInThePast",
        "The gathering is already scheduled in the past and cannot accept invitations.");

    public static readonly Error Expired = Error.Problem(
        "Gatherings.Expired",
        "The gathering has expired and is no longer active.");

    public static readonly Error InvalidGatheringType = Error.Problem(
        "Gatherings.InvalidGatheringType",
        "The specified gathering type is invalid or unsupported.");
}