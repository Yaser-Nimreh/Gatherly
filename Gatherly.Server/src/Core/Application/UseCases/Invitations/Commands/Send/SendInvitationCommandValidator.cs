using FluentValidation;

namespace Application.UseCases.Invitations.Commands.Send;

public sealed class SendInvitationCommandValidator : AbstractValidator<SendInvitationCommand>
{
    public SendInvitationCommandValidator()
    {
        RuleFor(invitation => invitation.MemberId)
            .NotEmpty()
            .WithMessage("MemberId is required.");

        RuleFor(invitation => invitation.GatheringId)
            .NotEmpty()
            .WithMessage("GatheringId is required.");
    }
}