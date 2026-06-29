using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_dotnet.Dtos.Project;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Extensions;
using mvc_dotnet.Services;
using mvc_dotnet.Services.Interfaces;


namespace mvc_dotnet.Controller;


[ApiController]
[Route("api/[controller]")]
public class ProjectController(IProjectService projectService, ILogger<ProjectController> logger) : ControllerBase
{
    [HttpPost("create")]
    [Authorize] // this enforces that the user must be authenticated to access this endpoint jwt token must be provided in the Authorization header
    public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)

    {
        var userId = User.GetUserId();
        logger.LogInformation("Creating project for user {UserId} with name {ProjectName}", userId, createProjectDto.ProjectName);
        var project = await projectService.CreateProjectAsync(createProjectDto, userId);
        return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllProjects()
    {

        var projects = await projectService.GetAllProjectsAsync();
        return Ok(projects);
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var project = await projectService.GetProjectByIdAsync(id);
        return Ok(project);
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var result = await projectService.DeleteProjectAsync(id);
        if (!result) throw new NotFoundException($"Project with id {id} not found.", id);
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectDto updateProjectDto)
    {
        var result = await projectService.UpdateProjectAsync(id, updateProjectDto);
        if (result is null) throw new NotFoundException($"Project with id {id} not found.", id);
        return Ok(result);
    }
    [HttpGet("owner/{ownerId}")]
    [Authorize]

    public async Task<IActionResult> GetProjectsByOwnerId(Guid ownerId)

    {
        logger.LogInformation("Getting projects for owner {OwnerId}", ownerId);
        var projects = await projectService.GetProjectsByOwnerIdAsync(ownerId);
        return Ok(projects);
    }

[HttpGet("/my-projects")]
[Authorize]
public async Task<IActionResult> GetProjectsByUserId()
    {
        var userId =  User.GetUserId();
        logger.LogInformation("Getting projects for user {UserId}", userId);
        var projects = await projectService.GetProjectsByUserIdAsync(userId);
        return Ok(projects);
    }


    [HttpGet("{projectId}/isUserInProject/{userId}")]
    [Authorize]


    public async Task<IActionResult> IsUserInProject(Guid projectId , Guid userId)
    {
        var isInProject = await projectService.IsUserInProjectAsync(projectId, userId);
        return Ok(new { IsInProject = isInProject });
    }
    [HttpGet("{projectId}/progress")]
    [Authorize]

    public async Task<IActionResult> GetProjectProgress(Guid projectId)
    {
        var result =  await projectService.GetProjectProgressAsync(projectId);
        return Ok(new { Progress = result });
    }
}