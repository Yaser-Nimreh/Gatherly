using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Invitations.Commands.Send;

internal sealed class SendInvitationCommandHandler(
    IMemberRepository memberRepository,
    IGatheringRepository gatheringRepository,
    IInvitationRepository invitationRepository,
    IUnitOfWork unitOfWork,
    IEmailService emailService)
    : ICommandHandler<SendInvitationCommand>
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IGatheringRepository _gatheringRepository = gatheringRepository;
    private readonly IInvitationRepository _invitationRepository = invitationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(SendInvitationCommand command, CancellationToken cancellationToken) =>
        await Result.Combine(
            Result.Create(
                await _memberRepository.GetByIdAsync(command.MemberId, cancellationToken),
                MemberErrors.NotFound(command.MemberId)),
            Result.Create(
                await _gatheringRepository.GetByIdWithCreatorAsync(command.GatheringId, cancellationToken),
                GatheringErrors.NotFound(command.GatheringId)))
            .Bind(tuple => tuple.Item2.SendInvitation(tuple.Item1).Value)
            .Tap(_invitationRepository.Add)
            .Tap(() => _unitOfWork.SaveChangesAsync(cancellationToken))
            .Tap(invitation => _emailService.SendInvitationSentEmailAsync(
                invitation.Member,
                invitation.Gathering,
                cancellationToken));

    public async Task<Result> Handle_Old(SendInvitationCommand command, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(command.MemberId, cancellationToken);

        if (member is null)
        {
            return Result.Failure(MemberErrors.NotFound(command.MemberId));
        }

        var gathering = await _gatheringRepository.GetByIdWithCreatorAsync(command.GatheringId, cancellationToken);

        if (gathering is null)
        {
            return Result.Failure(GatheringErrors.NotFound(command.GatheringId));
        }

        var invitationResult = gathering.SendInvitation(member);

        if (invitationResult.IsFailure)
        {
            return Result.Failure(invitationResult.Errors);
        }

        var invitation = invitationResult.Value;

        _invitationRepository.Add(invitation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _emailService.SendInvitationSentEmailAsync(member, gathering, cancellationToken);

        return Result.Success();
    }
}