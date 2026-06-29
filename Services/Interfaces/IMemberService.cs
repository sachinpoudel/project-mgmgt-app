using mvc_dotnet.Dtos.Member;
using mvc_dotnet.Enums;

namespace mvc_dotnet.Services.Interfaces;


public interface IMemberService
{
    Task<CreateMemberDto> CreateMemberInProjectAsync(CreateMemberDto createMemberDto, Guid userId);
    Task<MemberResponseDto> GetMemberByIdAsync(Guid memberId, Guid projectId);
    Task<List<MemberResponseDto>> GetAllProjectMembersAsync(Guid projectId);
    Task<MemberResponseDto> UpgradeMemberRoleAsync(Guid userId , Guid projectId , IncomingRoleDto roleDto);
    Task<bool> RemoveMemberFromProjectAsync(Guid userId , Guid projectId);
    Task<bool> IsMemberAsync(Guid memberId, Guid projectId);
}