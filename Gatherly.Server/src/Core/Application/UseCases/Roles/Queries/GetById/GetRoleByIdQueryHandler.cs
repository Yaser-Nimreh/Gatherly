using Application.Abstractions.Messaging;
using Application.UseCases.Roles.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Roles.Queries.GetById;

internal sealed class GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetRoleByIdQuery, RoleResponse>
{
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<Result<RoleResponse>> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(query.RoleId);

        if (role is null)
        {
            return Result.Failure<RoleResponse>(RoleErrors.NotFound(query.RoleId));
        }

        var response = new RoleResponse(
            role.Id,
            role.Name!,
            role.Description,
            role.IsSystemRole);

        return response;
    }
}