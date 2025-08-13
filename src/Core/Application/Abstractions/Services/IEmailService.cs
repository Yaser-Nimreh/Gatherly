using Domain.Entities;

namespace Application.Abstractions.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(Member member, CancellationToken cancellationToken = default);
    Task SendInvitationSentEmailAsync(Member member, Gathering gathering, CancellationToken cancellationToken = default);
    Task SendInvitationAcceptedEmailAsync(Gathering gathering, CancellationToken cancellationToken = default);
}