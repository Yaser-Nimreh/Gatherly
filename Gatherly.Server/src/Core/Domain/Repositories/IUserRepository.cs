using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(Email email);
    Task<IdentityResult> CreateAsync(User user);
}