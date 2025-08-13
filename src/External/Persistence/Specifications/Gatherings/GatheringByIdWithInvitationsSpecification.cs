using Domain.Entities;

namespace Persistence.Specifications.Gatherings;

internal sealed class GatheringByIdWithInvitationsSpecification : Specification<Gathering>
{
    public GatheringByIdWithInvitationsSpecification(Guid id)
        : base(gathering => gathering.Id == id)
    {
        AddInclude(gathering => gathering.Invitations);
    }
}