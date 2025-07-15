using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Events.Members;
using Domain.Repositories;

namespace Application.UseCases.Members.Events;

public sealed class MemberRegisteredEventHandler(
    IMemberRepository memberRepository,
    IEmailService emailService)
    : IDomainEventHandler<MemberRegisteredEvent>
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IEmailService _emailService = emailService;

    public async Task Handle(MemberRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(notification.MemberId, cancellationToken);

        if (member is null)
        {
            return;
        }

        await _emailService.SendWelcomeEmailAsync(member, cancellationToken);
    }
}