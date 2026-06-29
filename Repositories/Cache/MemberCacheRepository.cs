using Microsoft.Extensions.Caching.Distributed;
using mvc_dotnet.Enums;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;

namespace mvc_dotnet.Repositories.Cache;


public class MemberCacheRepository(MemberRepository memberRepository, IDistributedCache cache, ILogger<MemberCacheRepository> logger) : IMemberRepository
{


    public async Task<List<ProjectMember>> GetAllProjectMembersAsync(Guid projectId)
    {
        var cacheKey = $"members:project:{projectId}";
        var cacheOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

        var members = await  cache.GetOrSetAsync(cacheKey, async () =>
        {
            logger.LogInformation("Cache Miss for project members. Fetching from database.");
            return await memberRepository.GetAllProjectMembersAsync(projectId);
        }, cacheOptions);
        return members ?? [];

    }
    public async Task<ProjectMember> CreateMemberInProjectAsync(ProjectMember projectMember)
    {
        logger.LogInformation("Invalidating cache for project members after creating a new member.");
        var cacheKey = $"members:project:{projectMember.ProjectId}";
        await cache.RemoveAsync(cacheKey); // this will invalidate the cache for the project members list, so next time it will fetch from database this prevents stale data from being returned after a new member is added
        return await memberRepository.CreateMemberInProjectAsync(projectMember);
    }
    public async Task<ProjectMember> GetProjectMemberByIdAsync(Guid userId, Guid projectId)
    {
 var cacheKey = $"members:{userId}:{projectId}";

 logger.LogInformation("Fetching project member from database for userId: {UserId}, projectId: {ProjectId}", userId, projectId);


 var project = await cache.GetOrSetAsync(cacheKey, async () =>
 {
     logger.LogInformation("Cache Miss for project member. Fetching from database.");
     return await memberRepository.GetProjectMemberByIdAsync(userId, projectId);
 });
 return project;
    }

    public Task<bool> IsMemberAsync(Guid memberId, Guid projectId)
    => memberRepository.IsMemberAsync(memberId, projectId);

    public Task<bool> RemoveMemberFromProjectAsync(Guid userId, Guid projectId) => memberRepository.RemoveMemberFromProjectAsync(userId, projectId);
   
    public Task<ProjectMember> UpgradeMemberRoleAsync(Guid userId, Guid projectId, MemberRole newRole) => memberRepository.UpgradeMemberRoleAsync(userId, projectId, newRole);

    public Task<ProjectMember?> CheckUserRoleInProjectAsync(Guid userId, Guid ProjectId)
   => memberRepository.CheckUserRoleInProjectAsync(userId, ProjectId);
}