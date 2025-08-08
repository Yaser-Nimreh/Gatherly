using Domain.Entities;

namespace Persistence.Specifications.Gatherings;

internal sealed class GatheringByIdWithCreatorSpecification : Specification<Gathering>
{
    public GatheringByIdWithCreatorSpecification(Guid gatheringId)
        : base(gathering => gathering.Id == gatheringId) 
    {
        AddInclude(gathering => gathering.Creator!);
    }
}