using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests.Users;

public sealed record LoginUserRequest(
    [Required, MaxLength(Email.MaxLength), EmailAddress] string Email, 
    [Required, DataType(DataType.Password)] string Password);