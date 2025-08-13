using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options.JwtToken;

public sealed class JwtTokenOptions
{
    [Required, MinLength(32, ErrorMessage = "SecretKey must be at least 32 characters for security.")]
    public string SecretKey { get; set; } = string.Empty; // The secret key used for signing tokens
    [Required, Url(ErrorMessage = "Issuer must be a valid URL.")]
    public string Issuer { get; set; } = string.Empty; // The issuer of the token
    [Required, Url(ErrorMessage = "Audience must be a valid URL.")]
    public string Audience { get; set; } = string.Empty; // The audience for which the token is intended
    [Required, Range(1, 100)]
    public int ExpirationInMinutes { get; set; } = 60; // Default token expiration time in minutes
}