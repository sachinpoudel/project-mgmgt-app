using mvc_dotnet.Dtos.Member;
using mvc_dotnet.Enums;
using mvc_dotnet.Models;

namespace mvc_dotnet.Repositories.Interface;


public interface IMemberRepository
{
    Task<ProjectMember> CreateMemberInProjectAsync( ProjectMember projectMember );
    Task<List<ProjectMember>> GetAllProjectMembersAsync(Guid projectId);
    Task<ProjectMember> UpgradeMemberRoleAsync(Guid userId , Guid projectId , MemberRole roleDto);
    Task<bool> RemoveMemberFromProjectAsync(Guid userId , Guid projectId);
    Task<ProjectMember> GetProjectMemberByIdAsync(Guid memberId, Guid projectId);
    Task<bool> IsMemberAsync(Guid memberId, Guid projectId);
    Task<ProjectMember?> CheckUserRoleInProjectAsync(Guid userId, Guid ProjectId);
}