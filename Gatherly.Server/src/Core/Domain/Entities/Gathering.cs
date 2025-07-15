using Domain.Enums;
using Domain.Errors;
using Domain.Events.Gatherings;
using Domain.Events.Invitations;
using Domain.Primitives;
using Domain.Results;

namespace Domain.Entities;

public sealed class Gathering : AggregateRoot
{
    private readonly List<Invitation> _invitations = [];
    private readonly List<Attendee> _attendees = [];

    private Gathering(
        Guid id,
        Member creator,
        GatheringType type,
        DateTime scheduledAt,
        string name,
        string? location)
        : base(id)
    {
        Creator = creator;
        Type = type;
        ScheduledAt = scheduledAt;
        Name = name;
        Location = location;
    }

    private Gathering() : base(Guid.Empty) { }

    public Member? Creator { get; private set; }
    public GatheringType Type { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime ScheduledAt { get; private set; }
    public string? Location { get; private set; }
    public int? MaximumNumberOfAttendees { get; private set; }
    public DateTime? InvitationsExpireAt { get; private set; }
    public int NumberOfAttendees { get; private set; }

    public IReadOnlyCollection<Invitation> Invitations => _invitations;
    public IReadOnlyCollection<Attendee> Attendees => _attendees;

    public static Result<Gathering> Create(
        Guid id,
        Member creator,
        GatheringType type,
        DateTime scheduledAt,
        string name,
        string? location,
        int? maximumNumberOfAttendees,
        int? invitationsValidBeforeInHours)
    {
        var _gathering = new Gathering(
            id,
            creator,
            type,
            scheduledAt,
            name,
            location);

        var gatheringResult = _gathering.CalculateGatheringTypeDetails(maximumNumberOfAttendees, invitationsValidBeforeInHours);

        if (gatheringResult.IsFailure)
        {
            return Result.Failure<Gathering>(gatheringResult.Error);
        }

        var gathering = gatheringResult.Value;

        gathering.Raise(new GatheringCreatedEvent(gathering.Id));

        return gathering;
    }

    private Result<Gathering> CalculateGatheringTypeDetails(
        int? maximumNumberOfAttendees,
        int? invitationsValidBeforeInHours)
    {
        switch (Type)
        {
            case GatheringType.WithFixedNumberOfAttendees:
                if (maximumNumberOfAttendees is null)
                {
                    return Result.Failure<Gathering>(GatheringErrors.MaximumNumberOfAttendeesIsRequired);
                }
                MaximumNumberOfAttendees = maximumNumberOfAttendees;
                break;

            case GatheringType.WithExpirationForInvitations:
                if (invitationsValidBeforeInHours is null)
                {
                    return Result.Failure<Gathering>(GatheringErrors.InvitationsValidBeforeInHoursIsRequired);
                }
                InvitationsExpireAt = ScheduledAt.AddHours(-invitationsValidBeforeInHours.Value);
                break;

            default:
                return Result.Failure<Gathering>(GatheringErrors.InvalidGatheringType);
        }

        return this;
    }

    public Result<Invitation> SendInvitation(Member member)
    {
        if (Creator?.Id == member.Id)
        {
            return Result.Failure<Invitation>(GatheringErrors.InvitationForCreatorNotAllowed);
        }

        if (ScheduledAt < DateTime.UtcNow)
        {
            return Result.Failure<Invitation>(GatheringErrors.AlreadyScheduledInThePast);
        }

        var invitation = new Invitation(Guid.NewGuid(), member, this);

        _invitations.Add(invitation);

        return invitation;
    }

    public Result<Attendee> AcceptInvitation(Invitation invitation) 
    {
        var reachedMaximumNumberOfAttendees =
            Type == GatheringType.WithFixedNumberOfAttendees &&
            NumberOfAttendees == MaximumNumberOfAttendees;

        var reachedInvitationsExpiration =
            Type == GatheringType.WithExpirationForInvitations &&
            InvitationsExpireAt < DateTime.UtcNow;

        var expired = reachedMaximumNumberOfAttendees || reachedInvitationsExpiration;

        if (expired)
        {
            invitation.Expire();

            return Result.Failure<Attendee>(GatheringErrors.Expired);
        }

        var attendee = invitation.Accept();

        Raise(new InvitationAcceptedEvent(invitation.Id, Id));

        _attendees.Add(attendee);

        NumberOfAttendees++;

        return attendee;
    }
}