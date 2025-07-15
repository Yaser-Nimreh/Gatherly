using Application.UseCases.Gatherings.Commands.Create;
using Application.UseCases.Gatherings.Queries.GetById;
using Application.UseCases.Gatherings.Responses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Requests.Gatherings;

namespace Presentation.Controllers;

[Route("api/gatherings")]
public sealed class GatheringsController(ISender sender) : ApiController(sender)
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GatheringResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetGatheringByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGatheringRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateGatheringCommand>();

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction("GetById", new { id = result.Value }, result.Value);
    }
}