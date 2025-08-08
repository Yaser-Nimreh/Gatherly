using Domain.Entities;

namespace Persistence.Specifications.Gatherings;

internal sealed class GatheringByIdSplitSpecification : Specification<Gathering>
{
    public GatheringByIdSplitSpecification(Guid id)
        : base(gathering => gathering.Id == id)
    {
        AddInclude(gathering => gathering.Creator!);
        AddInclude(gathering => gathering.Attendees);
        AddInclude(gathering => gathering.Invitations);

        IsSplitQuery = true;
    }
}