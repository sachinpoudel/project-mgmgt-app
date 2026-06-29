using Microsoft.Extensions.Caching.Distributed;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;

namespace mvc_dotnet.Repositories.Cache;


public class ProjectCacheRepository(IDistributedCache cache, ILogger<ProjectCacheRepository> logger, ProjectRepository projectRepository) : IProjectRepository
{


private readonly DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(1));

    public Task<Project> CreateProjectAsync(Project project, ProjectMember membership, Guid ownerId)
    {
       var cacheKey = $"projects:{project.Id}";
        logger.LogInformation("Invalidating cache for project after creating a new project.");
        cache.RemoveAsync(cacheKey); // this will invalidate the cache for the project, so next time it will fetch from database this prevents stale data from being returned after a new project is added
        return projectRepository.CreateProjectAsync(project, membership, ownerId);
    }

    public Task<bool> DeleteProjectAsync(Guid projectId)
    {
        var cacheKey = $"projects:{projectId}";
        logger.LogInformation("Invalidating cache for project after deleting.");
        cache.RemoveAsync(cacheKey);
        return projectRepository.DeleteProjectAsync(projectId);
    }

    public Task<bool> ExisProjectAsync(Guid projectId)
  => projectRepository.ExisProjectAsync(projectId);

    public Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        const string cacheKey = "projects:all";
        logger.LogInformation("Cache lookup for key: {CacheKey}", cacheKey);
        return cache.GetOrSetAsync(cacheKey, () =>
        {
            logger.LogInformation("Cache miss for {CacheKey}. Fetching from database.", cacheKey);
            return projectRepository.GetAllProjectsAsync();
        }, cacheEntryOptions)!;
    }

    public Task<Project> GetProjectByIdAsync(Guid projectId)
    {
        var cacheKey = $"projects:{projectId}";
        logger.LogInformation("Cache lookup for key: {CacheKey}", cacheKey);
        var result =  cache.GetOrSetAsync(cacheKey, () =>
        {
            logger.LogInformation("Cache miss for {CacheKey}. Fetching from database.", cacheKey);
            return projectRepository.GetProjectByIdAsync(projectId);
        }, cacheEntryOptions);
        return result!;

    }

    public Task<int> GetProjectProgressAsync(Guid projectId)
    {
       var cacheKey = $"projects:progress:{projectId}";
        logger.LogInformation("Cache lookup for key: {CacheKey}", cacheKey);
        return cache.GetOrSetAsync(cacheKey, () =>
        {
            logger.LogInformation("Cache miss for {CacheKey}. Fetching from database.", cacheKey);
            return projectRepository.GetProjectProgressAsync(projectId);
        }, cacheEntryOptions)!;
    }

    public Task<IEnumerable<Project>> GetProjectsByOwnerIdAsync(Guid ownerId)
    {
        var cacheKey = $"projects:owner:{ownerId}";
        logger.LogInformation("Cache lookup for key: {CacheKey}", cacheKey);
        return cache.GetOrSetAsync(cacheKey, () =>
        {
            logger.LogInformation("Cache miss for {CacheKey}. Fetching from database.", cacheKey);
            return projectRepository.GetProjectsByOwnerIdAsync(ownerId);
        }, cacheEntryOptions)!;
    }

    public Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId)
    {
    var cacheKey = $"projects:user:{userId}";
        logger.LogInformation("Cache lookup for key: {CacheKey}", cacheKey);
        return cache.GetOrSetAsync(cacheKey, () =>
        {
            logger.LogInformation("Cache miss for {CacheKey}. Fetching from database.", cacheKey);
            return projectRepository.GetProjectsByUserIdAsync(userId);
        }, cacheEntryOptions)!;
    }

    public Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId)
    => projectRepository.IsUserInProjectAsync(projectId, userId);

    public Task<Project> UpdateProjectAsync(Guid projectId, Project updatedProject)
    {
        var cacheKey = $"projects:{projectId}";
        logger.LogInformation("Invalidating cache for project after updating.");
        cache.RemoveAsync(cacheKey);
        return projectRepository.UpdateProjectAsync(projectId, updatedProject);
    }
}