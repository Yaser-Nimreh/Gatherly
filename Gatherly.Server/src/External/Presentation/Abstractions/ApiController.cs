using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

    protected IActionResult HandleFailure(Result result) =>
        result.IsSuccess
            ? throw new InvalidOperationException("HandleFailure called on a successful result.")
            : result.Error switch
            {
                ValidationError validationError => BadRequest(
                    CreateProblemDetails(
                        title: "Validation Error",
                        error: validationError,
                        status: StatusCodes.Status400BadRequest,
                        errors: validationError.Errors)),

                _ => BadRequest(
                    CreateProblemDetails(
                        title: "Bad Request",
                        error: result.Error,
                        status: GetStatusCode(result.Error.Type)))
            };

    private static ProblemDetails CreateProblemDetails(
        string title,
        Error error,
        int status,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Description,
            Status = status,
            Extensions = { { nameof(errors), errors ?? [] } }
        };

    private static int GetStatusCode(ErrorType type) => type switch
    {
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Problem => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };
}