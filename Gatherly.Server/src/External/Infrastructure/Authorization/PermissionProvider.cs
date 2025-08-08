using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        var roles = await _dbContext.Set<User>()
            .Include(user => user.Roles)
            .ThenInclude(user => user.Permissions)
            .Where(user => user.Id == userId)
            .Select(user => user.Roles)
            .ToArrayAsync();

        return [.. roles
            .SelectMany(role => role)
            .SelectMany(role => role.Permissions)
            .Select(role => role.Name)];
    }
}