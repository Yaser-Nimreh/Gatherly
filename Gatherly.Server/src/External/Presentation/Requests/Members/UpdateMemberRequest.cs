using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests.Members;

public sealed record UpdateMemberRequest(
    [Required, MaxLength(FirstName.MaxLength)] string FirstName,
    [Required, MaxLength(LastName.MaxLength)] string LastName,
    [Required, MaxLength(Email.MaxLength), EmailAddress] string Email);