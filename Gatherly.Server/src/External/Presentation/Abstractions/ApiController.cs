using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Infrastructure;

namespace Presentation.Abstractions;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

    protected static IActionResult HandleFailure(Result result)
    {
        var details = ApiResponseHelper.CreateProblemDetails(result);
        return new ObjectResult(details) { StatusCode = details.Status };
    }
}