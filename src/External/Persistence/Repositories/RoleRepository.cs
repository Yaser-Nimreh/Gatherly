using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class RoleRepository(
    RoleManager<Role> roleManager,
    UserManager<User> userManager) 
    : IRoleRepository
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<IEnumerable<Role>> ListByUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var roleNames = await _userManager.GetRolesAsync(user);

        if (roleNames.Count == 0)
        {
            return [];
        }

        var roles = await _roleManager.Roles
            .Include(role => role.Permissions)
            .Where(role => roleNames.Contains(role.Name!))
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task<Role?> GetByIdAsync(Guid id) =>
        await _roleManager.FindByIdAsync(id.ToString());

    public async Task<Role?> GetByNameAsync(string roleName) =>
        await _roleManager.FindByNameAsync(roleName);

    public async Task<IdentityResult> CreateAsync(Role role) =>
        await _roleManager.CreateAsync(role);
}