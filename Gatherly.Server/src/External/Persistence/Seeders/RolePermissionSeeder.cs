using Domain.Abstractions;
using Domain.Entities;
using Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Permission = Domain.Enums.Permission;
using Role = Domain.Enums.Role;

namespace Persistence.Seeders;

internal sealed class RolePermissionSeeder(ApplicationDbContext dbContext) : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task SeedAsync()
    {
        if (await _dbContext.Set<RolePermission>().AnyAsync()) { return; }

        var rolePermissions = new[]
        {
            Create(Role.Registered, Permission.ReadMember),
            Create(Role.Registered, Permission.UpdateMember),
            // Add all other role-permission mappings here as needed
        };

        await _dbContext.Set<RolePermission>().AddRangeAsync(rolePermissions);
        await _dbContext.SaveChangesAsync();
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission
        {
            RoleId = GuidGenerator.FromEnumeration(role),
            PermissionId = GuidGenerator.FromEnum(permission)
        };
    }
}