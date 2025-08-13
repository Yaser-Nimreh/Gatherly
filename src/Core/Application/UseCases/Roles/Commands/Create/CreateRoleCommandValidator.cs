using FluentValidation;

namespace Application.UseCases.Roles.Commands.Create;

internal sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(role => role.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");

        RuleFor(role => role.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}