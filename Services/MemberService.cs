using Microsoft.AspNetCore.Http.HttpResults;
using mvc_dotnet.Dtos.Member;
using mvc_dotnet.Enums;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Service;


public class MemberService(IMemberRepository memberRepository, IUserRepository userRepository, IProjectRepository projectRepository) : IMemberService
{
    public async Task<CreateMemberDto> CreateMemberInProjectAsync(CreateMemberDto createMemberDto, Guid userId)
    {

        if (!await userRepository.ExistsUserAsync(createMemberDto.UserId)) throw new BadRequestException("Invalid UserId");

        if (!await projectRepository.ExisProjectAsync(createMemberDto.ProjectId)) throw new BadRequestException("Invalid ProjectId");


        var alreadyMember = await projectRepository.IsUserInProjectAsync(createMemberDto.ProjectId, createMemberDto.UserId);
        if (alreadyMember) throw new ConflictException("User is already a member of the project");


        if (createMemberDto.Role == MemberRole.Owner) throw new BadRequestException("Cannot assign owner role to a member");

        var user = await memberRepository.CheckUserRoleInProjectAsync(userId, createMemberDto.ProjectId);
        if (user == null || user.Role != MemberRole.Owner) throw new UnAuthorizedException("Only project owners can add members to the project");


        var newMember = new ProjectMember
        {
            ProjectId = createMemberDto.ProjectId,
            UserId = createMemberDto.UserId,
            Role = createMemberDto.Role,
            JoinedAt = DateTime.UtcNow
        };
        var result = await memberRepository.CreateMemberInProjectAsync(newMember);
        if (result is null) throw new ArgumentException("Failed to add member to project", $"{createMemberDto.ProjectId} - {createMemberDto.UserId}");
        return new CreateMemberDto
        {
            ProjectId = result.ProjectId,
            UserId = result.UserId,
            Role = result.Role,
            // FirstName = result.User.FirstName,
            // LastName = result.User.LastName
        };
    }

    public async Task<List<MemberResponseDto>> GetAllProjectMembersAsync(Guid projectId)
    {
        var isProjectIdValid = await projectRepository.ExisProjectAsync(projectId);
        if (!isProjectIdValid) throw new BadRequestException("Invalid projectId");
        var member = await memberRepository.GetAllProjectMembersAsync(projectId);

        return member.Select(m => new MemberResponseDto
        {
            MemberId = m.Id,
            ProjectId = m.ProjectId,
            UserId = m.UserId,
            Role = m.Role,
            JoinedAt = m.JoinedAt,
            FirstName = m.User.FirstName,
            LastName = m.User.LastName
        }).ToList();
    }

    public async Task<MemberResponseDto> GetMemberByIdAsync(Guid memberId, Guid projectId)
    {
        var isMemberIdValid = await memberRepository.IsMemberAsync(memberId, projectId);
        if (!isMemberIdValid) throw new BadRequestException("Invalid memberId");
        var member = await memberRepository.GetProjectMemberByIdAsync(memberId, projectId);
        if (member == null) throw new NotFoundException("Member not found", memberId.ToString());
        return new MemberResponseDto
        {
            MemberId = member.Id,
            ProjectId = member.ProjectId,
            UserId = member.UserId,
            Role = member.Role,
            JoinedAt = member.JoinedAt,
            FirstName = member.User.FirstName,
            LastName = member.User.LastName
        };
    }

    public async Task<bool> IsMemberAsync(Guid userId, Guid projectId)
    {
        if (userId == Guid.Empty || projectId == Guid.Empty) throw new BadRequestException("User ID and Project ID are required");
        var userExists = await userRepository.ExistsUserAsync(userId);
        if (!userExists) throw new BadRequestException("User not found");
        var projectExists = await projectRepository.ExisProjectAsync(projectId);
        if (!projectExists) throw new BadRequestException("Project not found");
        var result = await memberRepository.IsMemberAsync(userId, projectId); // returns true if the user is a member of the project, false otherwise
        return result;
    }

    public async Task<bool> RemoveMemberFromProjectAsync(Guid userId, Guid projectId)
    {
        if (userId == Guid.Empty || projectId == Guid.Empty) throw new BadRequestException("User ID and Project ID are required");
        var result = await memberRepository.RemoveMemberFromProjectAsync(userId, projectId);
        if (result == false) throw new OperationFailedException("Failed to remove member from project");
        return result;
    }

    public async Task<MemberResponseDto> UpgradeMemberRoleAsync(Guid userId, Guid projectId, IncomingRoleDto roleDto)
    {
        if (userId == Guid.Empty || projectId == Guid.Empty) throw new NotFoundException("User ID and Project ID are required", $"{userId} - {projectId}");
        var result = await memberRepository.UpgradeMemberRoleAsync(userId, projectId, roleDto.Role);
        if (result == null) throw new NotFoundException("Failed to upgrade member role", $"{userId} - {projectId}");
        return new MemberResponseDto
        {
            MemberId = result.Id,
            ProjectId = result.ProjectId,
            UserId = result.UserId,
            Role = result.Role,
            JoinedAt = result.JoinedAt,
            FirstName = result.User.FirstName,
            LastName = result.User.LastName
        };
    }
}