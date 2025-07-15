using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Events.Invitations;
using Domain.Repositories;

namespace Application.UseCases.Invitations.Events;

public sealed class InvitationAcceptedEventHandler(
    IGatheringRepository gatheringRepository,
    IEmailService emailService)
    : IDomainEventHandler<InvitationAcceptedEvent>
{
    private readonly IGatheringRepository _gatheringRepository = gatheringRepository;
    private readonly IEmailService _emailService = emailService;

    public async Task Handle(InvitationAcceptedEvent notification, CancellationToken cancellationToken)
    {
        var gathering = await _gatheringRepository.GetByIdWithCreatorAsync(notification.GatheringId, cancellationToken);

        if (gathering is null)
        {
            return;
        }

        await _emailService.SendInvitationAcceptedEmailAsync(gathering, cancellationToken);
    }
}