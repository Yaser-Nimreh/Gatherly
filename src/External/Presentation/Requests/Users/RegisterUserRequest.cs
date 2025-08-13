using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests.Users;

public sealed record RegisterUserRequest(
    [Required, MaxLength(FirstName.MaxLength)] string FirstName,
    [Required, MaxLength(LastName.MaxLength)] string LastName,
    [Required, MaxLength(Email.MaxLength), EmailAddress] string Email,
    [Required, DataType(DataType.Password)] string Password,
    [Required] string ConfirmPassword,
    [Required, MinLength(UserName.MinLength), MaxLength(UserName.MaxLength)] string UserName,
    [Required, MaxLength(PhoneNumber.MaxLength), Phone] string PhoneNumber);