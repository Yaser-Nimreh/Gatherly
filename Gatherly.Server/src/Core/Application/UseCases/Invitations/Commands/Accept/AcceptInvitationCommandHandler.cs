using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Invitations.Commands.Accept;

public sealed class AcceptInvitationCommandHandler(
    IGatheringRepository gatheringRepository,
    IAttendeeRepository attendeeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AcceptInvitationCommand>
{
    private readonly IGatheringRepository _gatheringRepository = gatheringRepository;
    private readonly IAttendeeRepository _attendeeRepository = attendeeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AcceptInvitationCommand command, CancellationToken cancellationToken)
    {
        var gathering = await _gatheringRepository.GetByIdWithCreatorAsync(command.GatheringId, cancellationToken);

        if (gathering is null)
        {
            return Result.Failure(GatheringErrors.NotFound(command.GatheringId));
        }

        var invitation = gathering.Invitations.FirstOrDefault(i => i.Id == command.InvitationId);

        if (invitation is null)
        {
            return Result.Failure(InvitationErrors.NotFound(command.InvitationId));
        }

        if (invitation.Status != InvitationStatus.Pending)
        {
            return invitation.Status switch
            {
                InvitationStatus.Accepted => Result.Failure(InvitationErrors.AlreadyAccepted),
                InvitationStatus.Rejected => Result.Failure(InvitationErrors.AlreadyRejected),
                InvitationStatus.Expired => Result.Failure(InvitationErrors.AlreadyExpired),
                _ => Result.Failure(InvitationErrors.InvalidStatus)
            };
        }

        var attendeeResult = gathering.AcceptInvitation(invitation);

        if (attendeeResult.IsSuccess)
        {
            var attendee = attendeeResult.Value;
            _attendeeRepository.Add(attendee);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}