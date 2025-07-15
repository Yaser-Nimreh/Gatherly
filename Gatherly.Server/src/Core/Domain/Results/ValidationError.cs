namespace Domain.Results;

public sealed class ValidationError(Error[] errors) : Error(
        "Validation.General",
        "One or more validation errors occurred",
        ErrorType.Validation)
{
    public Error[] Errors { get; } = errors;

    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new([.. results.Where(r => r.IsFailure).Select(r => r.Error)]);
}