using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Persistence.Data;
using Persistence.Infrastructure;

namespace Persistence.Repositories;

public sealed class CachedMemberRepository(
    IMemberRepository decorated,
    IDistributedCache distributedCache,
    ApplicationDbContext dbContext) 
    : IMemberRepository
{
    private readonly IMemberRepository _decorated = decorated;
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Member-{id}";

        var cachedMember = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

        Member? member;

        if (string.IsNullOrEmpty(cachedMember))
        {
            member = await _decorated.GetByIdAsync(id, cancellationToken);

            if (member is null)
            {
                return member;
            }

            var serializedMember = JsonConvert.SerializeObject(member);
            
            await _distributedCache.SetStringAsync(cacheKey, serializedMember, cancellationToken);

            return member;
        }

        member = JsonConvert.DeserializeObject<Member>(
            cachedMember,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            });

        if (member is not null)
        {
            _dbContext.Set<Member>().Attach(member);
        }

        return member;
    }

    public async Task<Member?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        await _decorated.GetByEmailAsync(email, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) =>
        await _decorated.IsEmailUniqueAsync(email, cancellationToken);

    public void Add(Member member) => _decorated.Add(member);

    public void Update(Member member) => _decorated.Update(member);
}