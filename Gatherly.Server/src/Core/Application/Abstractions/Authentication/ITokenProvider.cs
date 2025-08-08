using Domain.Entities;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    Task<string> CreateAsync(User user);
}