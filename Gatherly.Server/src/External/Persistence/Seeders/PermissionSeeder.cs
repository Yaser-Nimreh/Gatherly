using Domain.Abstractions;
using Domain.Entities;
using Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Seeders;

internal sealed class PermissionSeeder(ApplicationDbContext dbContext) : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task SeedAsync()
    {
        // Check if permissions already seeded
        if (await _dbContext.Set<Permission>().AnyAsync())
        {
            return; // Already seeded
        }

        var permissions = Enum.GetValues<Domain.Enums.Permission>()
            .Select(permission => new Permission
            {
                Id = GuidGenerator.FromEnum(permission),
                Name = permission.ToString()
            });

        await _dbContext.Set<Permission>().AddRangeAsync(permissions);
        await _dbContext.SaveChangesAsync();
    }
}