using Domain.ValueObjects;
using FluentValidation;

namespace Application.UseCases.Members.Commands.Update;

internal sealed class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(member => member.MemberId)
            .NotEmpty().WithMessage("Member ID is required.");

        RuleFor(member => member.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(FirstName.MaxLength).WithMessage($"First name must not exceed {FirstName.MaxLength} characters.");

        RuleFor(member => member.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(LastName.MaxLength).WithMessage($"Last name must not exceed {LastName.MaxLength} characters.");
        
        RuleFor(member => member.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(Email.MaxLength).WithMessage($"Email must not exceed {Email.MaxLength} characters.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}