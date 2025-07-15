using Domain.Abstractions;

namespace Domain.Events.Gatherings;

public sealed record GatheringCreatedEvent(Guid GatheringId) : IDomainEvent;