using Domain.ValueObjects;
using FluentValidation;

namespace Application.UseCases.Members.Commands.Register;

public sealed class RegisterMemberCommandValidator : AbstractValidator<RegisterMemberCommand>
{
    public RegisterMemberCommandValidator()
    {
        RuleFor(member => member.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(FirstName.MaxLength).WithMessage($"First name must not exceed {FirstName.MaxLength} characters.");
        
        RuleFor(member => member.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(LastName.MaxLength).WithMessage($"Last name must not exceed {LastName.MaxLength} characters.");

        RuleFor(member => member.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}