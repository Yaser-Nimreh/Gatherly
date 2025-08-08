namespace Infrastructure.Options.JwtToken;

public sealed class JwtTokenOptions
{
    public string SecretKey { get; set; } = string.Empty; // The secret key used for signing tokens
    public string Issuer { get; set; } = string.Empty; // The issuer of the token
    public string Audience { get; set; } = string.Empty; // The audience for which the token is intended
    public int ExpirationInMinutes { get; set; } = 60; // Default token expiration time in minutes
}