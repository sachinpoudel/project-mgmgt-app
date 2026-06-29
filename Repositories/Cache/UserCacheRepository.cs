using Microsoft.Extensions.Caching.Distributed;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;

namespace mvc_dotnet.Repositories.Cache;



 public class UserCacheRepository(
    IDistributedCache cache,
    UserRepository userRepository,
    ILogger<UserCacheRepository> logger) : IUserRepository
{
    private static readonly DistributedCacheEntryOptions CacheOptions =
        new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

    // ── Cached reads ──────────────────────────────────────────────

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        const string cacheKey = "users:all";
        logger.LogInformation("Cache lookup for key: {CacheKey}", cacheKey);
        return cache.GetOrSetAsync(cacheKey, () =>
        {
            logger.LogInformation("Cache miss for {CacheKey}. Fetching from database.", cacheKey);
            return userRepository.GetAllUsersAsync();
        }, CacheOptions)!;
    }

    public Task<User?> GetUserByEmailAsync(string email)
   => userRepository.GetUserByEmailAsync(email);

    public Task<User?> GetUserByIdAsync(Guid id)
    {
        var cacheKey = $"users:id:{id}";
        logger.LogInformation("Cache lookup for key: {CacheKey}", cacheKey);
        return cache.GetOrSetAsync(cacheKey, () =>
        {
            logger.LogInformation("Cache miss for {CacheKey}. Fetching from database.", cacheKey);
            return userRepository.GetUserByIdAsync(id);
        }, CacheOptions);
    }

    // ── Passthroughs (writes / actions — never cache these) ───────

    public Task<User> LoginUserAsync(string email, string password)
        => userRepository.LoginUserAsync(email, password);

    public Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password)
        => userRepository.RegisterUserAsync(firstName, lastName, email, password);

    public Task<bool> ExistsUserAsync(Guid userId)
        => userRepository.ExistsUserAsync(userId);

    public Task<bool> UpdateLastLoginAsync(Guid id)
        => userRepository.UpdateLastLoginAsync(id);

   
}