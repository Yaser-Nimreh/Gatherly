namespace Application.UseCases.Members.Responses;

public sealed record MemberResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);