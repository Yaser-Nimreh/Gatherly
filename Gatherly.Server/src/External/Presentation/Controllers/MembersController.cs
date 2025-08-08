using Application.UseCases.Members.Commands.Register;
using Application.UseCases.Members.Commands.Update;
using Application.UseCases.Members.Queries.GetById;
using Application.UseCases.Members.Responses;
using Domain.Enums;
using Domain.Results;
using Infrastructure.Authorization;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Constants;
using Presentation.Requests.Members;

namespace Presentation.Controllers;

[Route("api/members")]
[HasPermission(Permission.HeadMember)]
public sealed class MembersController(ISender sender) : ApiController(sender)
{
    [HasPermission(Permission.ReadMember)]
    [HttpGet("{id:guid}")]
    [Tags(Tags.Members)]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await Result
            .Create(new GetMemberByIdQuery(id))
            .Bind(query => Sender.Send(query, cancellationToken))
            .Match(
                onSuccess: response => Ok(response),
                onFailure: HandleFailure
            );
    }

    [HttpPost]
    [Tags(Tags.Members)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterMemberRequest request, CancellationToken cancellationToken)
    {
        return await Result
            .Create(request.Adapt<RegisterMemberCommand>())
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                onSuccess: id => CreatedAtAction("GetById", new { id }, id),
                onFailure: HandleFailure);
    }

    [HttpPut("{id:guid}")]
    [Tags(Tags.Members)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateMemberRequest request, CancellationToken cancellationToken)
    {
        return await Result
            .Create(request.Adapt<UpdateMemberCommand>() with { MemberId = id })
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                onSuccess: NoContent,
                onFailure: HandleFailure);
    }
}