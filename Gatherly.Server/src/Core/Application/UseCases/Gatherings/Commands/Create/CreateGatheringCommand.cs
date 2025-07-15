using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.UseCases.Gatherings.Commands.Create;

public sealed record CreateGatheringCommand(
    Guid MemberId,
    GatheringType Type,
    DateTime ScheduledAt,
    string Name,
    string? Location,
    int? MaximumNumberOfAttendees,
    int? InvitationsValidBeforeInHours)
    : ICommand<Guid>;