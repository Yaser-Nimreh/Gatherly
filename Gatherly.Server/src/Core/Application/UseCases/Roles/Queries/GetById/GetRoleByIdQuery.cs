using Application.Abstractions.Messaging;
using Application.UseCases.Roles.Responses;

namespace Application.UseCases.Roles.Queries.GetById;

public sealed record GetRoleByIdQuery(Guid RoleId) : IQuery<RoleResponse>;