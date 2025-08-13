using Application.Abstractions.Messaging;

namespace Application.UseCases.Invitations.Commands.Send;

public sealed record SendInvitationCommand(Guid MemberId, Guid GatheringId) : ICommand;