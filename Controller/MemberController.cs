using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_dotnet.Dtos.Member;
using mvc_dotnet.Enums;
using mvc_dotnet.Extensions;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Controller;

[ApiController]
[Route("api/[controller]")]

public class MemberController( IMemberService memberService) : ControllerBase
{
    [HttpPost("create")]
    [Authorize] // this enforces that the user must be authenticated to access this endpoint jwt token must be provided in the Authorization header
    public async Task<IActionResult> CreateMemberInProject(CreateMemberDto createMemberDto)

    {
        var userId = User.GetUserId();
        var result = await memberService.CreateMemberInProjectAsync(createMemberDto,userId);
         return CreatedAtAction(nameof(GetMemberById), new { id = result.MemberId }, result);
        
    }

    [HttpGet]
    [Route("api/{projectId}/member/{memberId}")]
    public async Task<IActionResult> GetMemberById(Guid memberId , Guid projectId)
    {
        var result = await memberService.GetMemberByIdAsync(memberId, projectId);
        return Ok(result);
    }

[HttpGet("all")]
[Authorize]

public async Task<IActionResult> GetAllProjectMembers(Guid projectId)
    {
        var result = await memberService.GetAllProjectMembersAsync(projectId);
        return Ok(result);
    }

[HttpDelete("/remove")]
[Authorize]

public async Task<IActionResult> RemoveMemberFromProject(Guid userId, Guid projectId)
    {
        var result = await memberService.RemoveMemberFromProjectAsync(userId, projectId);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPut("/upgrade-role")]
    [Authorize]
public async Task<IActionResult> UpgradeMemberRole(Guid userId, Guid projectId, IncomingRoleDto roleDto)
    {
        var result = await memberService.UpgradeMemberRoleAsync( userId,  projectId ,roleDto );
        return Ok(result);
    }}