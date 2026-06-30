using mvc_dotnet.Dtos.ProjectTask;
using mvc_dotnet.Models;

namespace mvc_dotnet.Services.Interfaces;


public interface IProjectTaskService
{
    Task<IEnumerable<ProjectTask>> GetAllTasksByProjectIdAsync(Guid projectId);
    Task<CreateTaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, Guid projectId, Guid userId);
    Task<TaskListDto> GetSingleTaskByIdAsync(Guid projectId, Guid taskId);
    Task<TaskListDto?> AssignTaskToMemberAsync(Guid projectId, Guid taskId, Guid assigneeId);
    Task<UpdateTaskStatusDto> UpdateStatusOfTaskAsync(UpdateProjectTaskDto updateProjectTaskDto, Guid taskId, Guid projectId);
    Task<bool> DeleteTaskByIdAsync(Guid projectId, Guid taskId, Guid userId);
    Task<UpdateProjectTaskDto> UpdateTaskAsync(Guid projectId, Guid taskId, UpdateProjectTaskDto updateProjectTaskDto);

}


