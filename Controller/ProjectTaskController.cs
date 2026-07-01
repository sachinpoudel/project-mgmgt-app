using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using mvc_dotnet.Dtos.ProjectTask;
using mvc_dotnet.Extensions;
using mvc_dotnet.Services;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Controller;

[ApiController]
[Route("api/[controller]")]


public class ProjectTaskController(IProjectTaskService projectTaskService) : ControllerBase
{
    [HttpGet("{projectId}/tasks")]
    public async Task<IActionResult> GetAllTaskByProjectId(Guid projectId)
    {
        var tasks = await projectTaskService.GetAllTasksByProjectIdAsync(projectId);
        return Ok(tasks);
    }


    [HttpPost("{projectId}/tasks")]
    public async Task<IActionResult> CreateTask(Guid projectId, CreateTaskDto createTaskDto)
    {
        var userId = User.GetUserId();
        var createdTask = await projectTaskService.CreateTaskAsync(createTaskDto, projectId, userId);
        return Ok(createdTask);
    }
    [HttpGet("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> GetSingleTaskById(Guid projectId, Guid taskId)
    {
        var task = await projectTaskService.GetSingleTaskByIdAsync(projectId, taskId);
        return Ok(task);
    }
    [HttpPatch("{projectId}/tasks/{taskId}/assign/{assigneeId}")]


    public async Task<IActionResult> AssignToMember(Guid projectId, Guid taskId, Guid assigneeId)
    {
        var updatedTask = await projectTaskService.AssignTaskToMemberAsync(projectId, taskId, assigneeId);
        return updatedTask is null ? NotFound() : Ok(updatedTask);
    }
    [HttpPatch("{projectId}/tasks/{taskId}/status")]
    public async Task<IActionResult> UpdateTaskStatus(Guid projectId, Guid taskId, UpdateProjectTaskDto updateProjectTaskDto)
    {
        var updatedTask = await projectTaskService.UpdateStatusOfTaskAsync(updateProjectTaskDto, taskId, projectId);
        return Ok(updatedTask);
    }
    [HttpDelete("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
    {
        var userId = User.GetUserId();
        var deleted = await projectTaskService.DeleteTaskByIdAsync(projectId, taskId, userId);
        return deleted ? Ok() : NotFound();
    }
    [HttpPut("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> UpdateTask(Guid projectId, Guid taskId, UpdateProjectTaskDto updateProjectTaskDto)
    {
        var updatedTask = await projectTaskService.UpdateTaskAsync(projectId, taskId, updateProjectTaskDto);
        return Ok(updatedTask); 
    }
}