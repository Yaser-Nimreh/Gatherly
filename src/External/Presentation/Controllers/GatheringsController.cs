using Application.UseCases.Gatherings.Commands.Create;
using Application.UseCases.Gatherings.Queries.GetById;
using Application.UseCases.Gatherings.Responses;
using Domain.Results;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Constants;
using Presentation.Requests.Gatherings;

namespace Presentation.Controllers;

[Route("api/gatherings")]
public sealed class GatheringsController(ISender sender) : ApiController(sender)
{
    [HttpGet("{id:guid}")]
    [Tags(Tags.Gatherings)]
    [ProducesResponseType(typeof(GatheringResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await Result
            .Create(new GetGatheringByIdQuery(id))
            .Bind(query => Sender.Send(query, cancellationToken))
            .Match(
                onSuccess: response => Ok(response),
                onFailure: HandleFailure);
    }

    [HttpPost]
    [Tags(Tags.Gatherings)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGatheringRequest request, CancellationToken cancellationToken)
    {
        return await Result
            .Create(request.Adapt<CreateGatheringCommand>())
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                onSuccess: id => CreatedAtAction("GetById", new { id }, id),
                onFailure: HandleFailure);
    }
}