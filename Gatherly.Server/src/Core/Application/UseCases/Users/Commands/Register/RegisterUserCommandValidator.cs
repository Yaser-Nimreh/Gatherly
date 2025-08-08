using Domain.ValueObjects;
using FluentValidation;

namespace Application.UseCases.Users.Commands.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(FirstName.MaxLength).WithMessage($"First name must not exceed {FirstName.MaxLength} characters.");
        
        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(LastName.MaxLength).WithMessage($"Last name must not exceed {LastName.MaxLength} characters.");
        
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(Email.MaxLength).WithMessage($"Email must not exceed {Email.MaxLength} characters.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(user => user.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(user => user.Password).WithMessage("Passwords do not match.");

        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(UserName.MinLength).WithMessage($"Username must be at least {UserName.MinLength} characters long.")
            .MaximumLength(UserName.MaxLength).WithMessage($"Username must not exceed {UserName.MaxLength} characters.");

        RuleFor(user => user.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
            .MaximumLength(PhoneNumber.MaxLength).WithMessage($"Phone number must not exceed {PhoneNumber.MaxLength} characters.");
    }
}