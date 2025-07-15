namespace Presentation.Requests.Members;

public sealed record RegisterMemberRequest(string FirstName, string LastName, string Email);