using Application.UseCases.Users.Commands.Login;
using Application.UseCases.Users.Commands.Register;
using Application.UseCases.Users.Queries.GetById;
using Application.UseCases.Users.Responses;
using Domain.Results;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Constants;
using Presentation.Requests.Users;

namespace Presentation.Controllers;

[Route("api/users")]
public sealed class UserController(ISender sender) : ApiController(sender)
{
    [HttpGet("{id:guid}")]
    [Tags(Tags.Users)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await Result
            .Create(new GetUserByIdQuery(id))
            .Bind(query => Sender.Send(query, cancellationToken))
            .Match(
                onSuccess: response => Ok(response),
                onFailure: HandleFailure);
    }

    [HttpPost]
    [Tags(Tags.Users)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        return await Result
            .Create(request.Adapt<RegisterUserCommand>())
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                onSuccess: id => CreatedAtAction("GetById", new { id }, id),
                onFailure: HandleFailure);
    }

    [HttpPost("login")]
    [Tags(Tags.Users)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        return await Result
            .Create(request.Adapt<LoginUserCommand>())
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                onSuccess: response => Ok(response),
                onFailure: HandleFailure);
    }
}