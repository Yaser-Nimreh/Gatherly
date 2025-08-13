using Application.Abstractions.Authentication;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Constants;
using Infrastructure.Options.JwtToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(
    IOptions<JwtTokenOptions> jwtTokenOptions,
    IRoleRepository roleRepository)
    : ITokenProvider
{
    private readonly JwtTokenOptions _jwtTokenOptions = jwtTokenOptions.Value;
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<string> CreateAsync(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(CustomClaims.FullName, user.FullName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _roleRepository.ListByUserAsync(user);
        if (roles.Any())
        {
            claims.AddRange(
                roles
                .Where(role => !string.IsNullOrWhiteSpace(role.Name))
                .Select(role => new Claim(ClaimTypes.Role, role.Name!))
            );
        }

        var permissions = roles
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .ToHashSet();

        if (permissions.Count != 0)
        {
            claims.AddRange(
                permissions
                .Where(permission => !string.IsNullOrEmpty(permission))
                .Select(permission => new Claim(CustomClaims.Permission, permission)));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtTokenOptions.ExpirationInMinutes),
            Issuer = _jwtTokenOptions.Issuer,
            Audience = _jwtTokenOptions.Audience,
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(tokenDescriptor);
    }
}