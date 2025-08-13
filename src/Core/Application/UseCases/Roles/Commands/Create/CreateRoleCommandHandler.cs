using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Roles.Commands.Create;

internal sealed class CreateRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoleCommand, Guid>
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var existingRole = await _roleRepository.GetByNameAsync(command.Name);

        if (existingRole is not null)
        {
            return Result.Failure<Guid>(RoleErrors.AlreadyExists(command.Name));
        }

        var role = Role.Create(
            Guid.NewGuid(),
            command.Name,
            command.Description,
            command.IsSystemRole);

        var result = await _roleRepository.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .Select(e => Result.Failure(e));

            var validationError = ValidationError.FromResults(errors);

            return Result.Failure<Guid>(validationError);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return role.Id;
    }
}