using Domain.Results;

namespace Domain.Errors;

public static class InvitationErrors
{
    public static Error NotFound(Guid invitationId) => Error.NotFound(
        "Invitations.NotFound",
        $"The invitation with Id = '{invitationId}' was not found.");

    public static readonly Error AlreadyExpired = Error.Conflict(
        "Invitations.AlreadyExpired",
        "The invitation has already expired.");

    public static readonly Error AlreadyAccepted = Error.Conflict(
        "Invitations.AlreadyAccepted",
        "The invitation has already been accepted.");

    public static readonly Error AlreadyRejected = Error.Conflict(
        "Invitations.AlreadyRejected",
        "The invitation has already been rejected.");

    public static readonly Error InvalidStatus = Error.Problem(
        "Invitations.InvalidStatus",
        "The invitation has an invalid status for this operation.");
}