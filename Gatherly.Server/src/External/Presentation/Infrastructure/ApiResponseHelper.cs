using Domain.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Infrastructure;

public static class ApiResponseHelper
{
    public static ProblemDetails CreateProblemDetails(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create problem details for a successful result.");
        }

        var error = result.Error;

        return new ProblemDetails
        {
            Title = GetTitle(error),
            Detail = GetDetail(error),
            Type = GetType(error.Type),
            Status = GetStatusCode(error.Type),
            Extensions = GetErrors(result)!
        };
    }

    public static IResult CreateProblemResult(Result result)
    {
        var details = CreateProblemDetails(result);
        return Results.Problem(
            title: details.Title,
            detail: details.Detail,
            type: details.Type,
            statusCode: details.Status,
            extensions: details.Extensions);
    }

    private static string GetTitle(Error error) =>
        error.Code;

    private static string GetDetail(Error error) =>
        error.Description;

    private static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

    private static Dictionary<string, object?>? GetErrors(Result result)
    {
        if (result.Error is not ValidationError validationError)
        {
            return null;
        }

        return new Dictionary<string, object?>
        {
            { "errors", validationError.Errors }
        };
    }
}