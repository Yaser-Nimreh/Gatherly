using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Repositories;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> ListByUserAsync(User user, CancellationToken cancellationToken = default);
    Task<Role?> GetByIdAsync(Guid id);
    Task<Role?> GetByNameAsync(string roleName);
    Task<IdentityResult> CreateAsync(Role role);
}