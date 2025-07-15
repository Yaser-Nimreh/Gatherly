using Domain.Enums;
using FluentValidation;

namespace Application.UseCases.Gatherings.Commands.Create;

public sealed class CreateGatheringCommandValidator : AbstractValidator<CreateGatheringCommand>
{
    public CreateGatheringCommandValidator()
    {
        RuleFor(gathering => gathering.MemberId)
            .NotEmpty()
            .WithMessage("MemberId is required.");

        RuleFor(gathering => gathering.Name)
            .NotEmpty()
            .WithMessage("Value is required.");

        RuleFor(gathering => gathering.ScheduledAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("ScheduledAt must be in the future.");

        RuleFor(gathering => gathering.Type)
            .IsInEnum()
            .WithMessage("Invalid gathering type.");

        When(gathering => gathering.Type == GatheringType.WithFixedNumberOfAttendees, () =>
        {
            RuleFor(gathering => gathering.MaximumNumberOfAttendees)
                .NotNull()
                .WithMessage("MaximumNumberOfAttendees is required for gatherings with a fixed number of attendees.")
                .GreaterThan(0)
                .WithMessage("MaximumNumberOfAttendees must be greater than zero.");
        });

        When(gathering => gathering.Type == GatheringType.WithExpirationForInvitations, () =>
        {
            RuleFor(gathering => gathering.InvitationsValidBeforeInHours)
                .NotNull()
                .WithMessage("InvitationsValidBeforeInHours is required for gatherings with invitations expiration.")
                .GreaterThan(0)
                .WithMessage("InvitationsValidBeforeInHours must be greater than zero.");
        });

        RuleFor(gathering => gathering.Location)
            .MaximumLength(200)
            .WithMessage("Location cannot exceed 200 characters.");
    }
}