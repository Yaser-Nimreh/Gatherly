using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UserRepository(UserManager<User> userManager) : IUserRepository
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) =>
        !await _userManager.Users.AnyAsync(user => user.Email == email.Value, cancellationToken: cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _userManager.FindByIdAsync(id.ToString());

    public async Task<User?> GetByEmailAsync(Email email) =>
        await _userManager.FindByEmailAsync(email.Value);

    public async Task<IdentityResult> CreateAsync(User user) =>
        await _userManager.CreateAsync(user);
}