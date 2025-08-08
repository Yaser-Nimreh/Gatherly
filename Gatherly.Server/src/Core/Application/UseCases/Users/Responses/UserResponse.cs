namespace Application.UseCases.Users.Responses;

public sealed record UserResponse(
    Guid Id,
    string FullName,
    string PhoneNumber,
    string UserName,
    string Email);