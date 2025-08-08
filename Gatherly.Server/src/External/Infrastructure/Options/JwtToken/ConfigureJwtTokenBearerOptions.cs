using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Options.JwtToken;

public sealed class ConfigureJwtTokenBearerOptions(IOptions<JwtTokenOptions> jwtTokenOptions) 
    : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtTokenOptions _jwtTokenOptions = jwtTokenOptions.Value;

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.RequireHttpsMetadata = false;

        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtTokenOptions.Issuer,
            ValidAudience = _jwtTokenOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey)),
            ClockSkew = TimeSpan.Zero // Disable clock skew for immediate token expiration validation
        };
    }
}