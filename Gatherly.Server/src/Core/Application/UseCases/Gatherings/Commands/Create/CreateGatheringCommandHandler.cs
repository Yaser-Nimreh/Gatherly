using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Gatherings.Commands.Create;

public sealed class CreateGatheringCommandHandler(
    IMemberRepository memberRepository,
    IGatheringRepository gatheringRepository,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateGatheringCommand, Guid>
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IGatheringRepository _gatheringRepository = gatheringRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateGatheringCommand command, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(command.MemberId, cancellationToken);

        if (member is null)
        {
            return Result.Failure<Guid>(MemberErrors.NotFound(command.MemberId));
        }

        var gatheringResult = Gathering.Create(
            Guid.NewGuid(),
            member,
            command.Type,
            command.ScheduledAt,
            command.Name,
            command.Location,
            command.MaximumNumberOfAttendees,
            command.InvitationsValidBeforeInHours);

        if (gatheringResult.IsFailure)
        {
            return Result.Failure<Guid>(gatheringResult.Error);
        }

        var gathering = gatheringResult.Value;

        _gatheringRepository.Add(gathering);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return gathering.Id;
    }
}