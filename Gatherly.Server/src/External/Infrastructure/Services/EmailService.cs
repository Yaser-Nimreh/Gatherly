using Application.Abstractions.Services;
using Domain.Entities;

namespace Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    public Task SendInvitationAcceptedEmailAsync(Gathering gathering, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task SendInvitationSentEmailAsync(Member member, Gathering gathering, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task SendWelcomeEmailAsync(Member member, CancellationToken cancellationToken = default) => Task.CompletedTask;
}