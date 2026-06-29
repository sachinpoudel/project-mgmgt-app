using Microsoft.EntityFrameworkCore;
using mvc_dotnet.Data;
using mvc_dotnet.Dtos.Member;
using mvc_dotnet.Enums;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;

namespace mvc_dotnet.Repositories;


public class MemberRepository(AppDbContext db) : IMemberRepository
{
    public async Task<bool> IsMemberAsync(Guid memberId, Guid projectId)
    {
        return await db.ProjectMembers.AnyAsync(m => m.Id == memberId && m.ProjectId == projectId); // returns true if member exists in the project, otherwise false
    }

    public async Task<ProjectMember> CreateMemberInProjectAsync(ProjectMember projectMember)
    {
        await db.ProjectMembers.AddAsync(projectMember);
        await db.SaveChangesAsync();
        return projectMember;
    }


    public async Task<List<ProjectMember>> GetAllProjectMembersAsync(Guid projectId)
    {
        return await db.ProjectMembers.Where(m => m.ProjectId == projectId).Include(m => m.User).ToListAsync();


    }

    public async Task<ProjectMember> GetProjectMemberByIdAsync(Guid memberId, Guid projectId)
    {
        var result = await db.ProjectMembers.Where(m => m.Id == memberId && m.ProjectId == projectId).Include(m => m.User).FirstOrDefaultAsync();
        return result;
    }

    public async Task<bool> RemoveMemberFromProjectAsync(Guid userId, Guid projectId)
    {
        var member = await db.ProjectMembers.FirstOrDefaultAsync(m => m.UserId == userId && m.ProjectId == projectId);
        db.Remove(member);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<ProjectMember> UpgradeMemberRoleAsync(Guid userId, Guid projectId, MemberRole roleDto)
    {
        var result = await db.ProjectMembers.Where(m => m.ProjectId == projectId).Include(m => m.Role).FirstOrDefaultAsync();
        result.Role = roleDto;
        await db.SaveChangesAsync();
        return result;
    }

    public async Task<ProjectMember?> CheckUserRoleInProjectAsync(Guid userId, Guid ProjectId)
    {
        var user = await db.ProjectMembers.FirstOrDefaultAsync(m => m.UserId == userId && m.ProjectId == ProjectId);
        return user;
    }
}