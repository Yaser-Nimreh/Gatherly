using Domain.Enums;
using Domain.Primitives;

namespace Domain.Entities;

public sealed class Invitation : SoftDeletableEntity
{
    internal Invitation(Guid id, Member member, Gathering gathering) : base(id)
    {
        MemberId = member.Id;
        GatheringId = gathering.Id;
        Member = member;
        Gathering = gathering;
        Status = InvitationStatus.Pending;
    }

    private Invitation() : base() { }

    public Guid GatheringId { get; private set; }
    public Guid MemberId { get; private set; }
    public Member Member { get; private set; }
    public Gathering Gathering { get; private set; }
    public InvitationStatus Status { get; private set; }

    internal void Expire()
    {
        Status = InvitationStatus.Expired;
    }

    internal Attendee Accept()
    {
        Status = InvitationStatus.Accepted;

        var attendee = new Attendee(this);

        return attendee;
    }
}