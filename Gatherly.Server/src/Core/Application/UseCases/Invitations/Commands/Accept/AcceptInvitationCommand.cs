using Application.Abstractions.Messaging;

namespace Application.UseCases.Invitations.Commands.Accept;

public sealed record AcceptInvitationCommand(Guid GatheringId, Guid InvitationId) : ICommand;