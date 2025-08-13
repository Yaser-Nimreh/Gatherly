using Domain.Abstractions;
using Domain.Entities;
using Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Seeders;

internal sealed class RoleSeeder(ApplicationDbContext dbContext) : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task SeedAsync()
    {
        if (await _dbContext.Set<Role>().AnyAsync()) { return; }

        var roles = Domain.Enums.Role.GetValues()
            .Select(role => new Role(
                GuidGenerator.FromEnumeration(role),
                role.Name,
                role.Description,
                role.IsSystemRole));

        await _dbContext.Set<Role>().AddRangeAsync(roles);
        await _dbContext.SaveChangesAsync();
    }
}