using Domain.Primitives;

namespace Domain.Events.Gatherings;

public sealed record GatheringCreatedEvent(Guid Id, Guid GatheringId) : DomainEvent(Id);