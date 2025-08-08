using Infrastructure.Authentication;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        // Reject if the user is not authenticated or doesn't have a user ID claim
        if (context.User?.Identity?.IsAuthenticated != true || !context.User.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
        {
            return;
        }

        var claimPermissions = context
            .User
            .Claims
            .Where(claim => claim.Type == CustomClaims.Permission)
            .Select(claim => claim.Value)
            .ToHashSet();

        if (claimPermissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
            return;
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        var permissionProvider = scope.ServiceProvider.GetRequiredService<PermissionProvider>();

        var userId = context.User.GetUserId();

        var permissions = await permissionProvider.GetForUserIdAsync(userId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}