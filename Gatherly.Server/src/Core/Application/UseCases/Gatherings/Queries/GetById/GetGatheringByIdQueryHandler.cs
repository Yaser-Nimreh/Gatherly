using Application.Abstractions.Messaging;
using Application.UseCases.Attendee.Responses;
using Application.UseCases.Gatherings.Responses;
using Application.UseCases.Invitations.Responses;
using Application.UseCases.Members.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Gatherings.Queries.GetById;

public sealed class GetGatheringByIdQueryHandler(IGatheringRepository gatheringRepository)
    : IQueryHandler<GetGatheringByIdQuery, GatheringResponse>
{
    private readonly IGatheringRepository _gatheringRepository = gatheringRepository;

    public async Task<Result<GatheringResponse>> Handle(GetGatheringByIdQuery query, CancellationToken cancellationToken)
    {
        var gathering = await _gatheringRepository.GetByIdAsync(query.GatheringId, cancellationToken);
        
        if (gathering is null)
        {
            return Result.Failure<GatheringResponse>(GatheringErrors.NotFound(query.GatheringId));
        }

        var response = new GatheringResponse(
            gathering.Id,
            gathering.Name,
            gathering.ScheduledAt,
            gathering.Location,
            gathering.Type,
            gathering.MaximumNumberOfAttendees,
            gathering.InvitationsExpireAt,
            gathering.NumberOfAttendees,
            new MemberResponse (
                gathering.Creator!.Id,
                gathering.Creator.FirstName!.Value,
                gathering.Creator.LastName!.Value,
                gathering.Creator.Email!.Value
                ),
            [.. gathering
                .Invitations
                .Select(invitation => new InvitationResponse(
                    invitation.Id,
                    invitation.GatheringId,
                    invitation.MemberId,
                    invitation.Status))],
            [.. gathering
                .Attendees
                .Select(attendee => new AttendeeResponse(
                    attendee.GatheringId,
                    attendee.MemberId))]);

        return response;
    }
}