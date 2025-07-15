using FluentValidation;

namespace Application.UseCases.Invitations.Commands.Accept;

public sealed class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
{
    public AcceptInvitationCommandValidator()
    {
        RuleFor(invitation => invitation.GatheringId)
            .NotEmpty()
            .WithMessage("GatheringId is required.");

        RuleFor(invitation => invitation.InvitationId)
            .NotEmpty()
            .WithMessage("InvitationId is required.");
    }
}