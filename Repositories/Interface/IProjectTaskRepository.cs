using mvc_dotnet.Enums;
using mvc_dotnet.Models;

namespace mvc_dotnet.Repositories.Interface;


public interface IProjectTaskRepository
{
    Task<IEnumerable<ProjectTask>> GetAllTasksByProjectIdAsync(Guid projectId);
    Task<ProjectTask> CreateTaskAsync(ProjectTask task);
    Task<ProjectTask?> GetSingleTaskByIdAsync(Guid projectId, Guid taskId);
    Task<bool> IsTaskExistsAsync(Guid projectId , Guid taskId);
    Task<ProjectTask?> AssignTaskToMemberAsync(Guid projectId , Guid taskId, Guid assigneeId);
    Task<ProjectTask?> UpdateStatusOfTaskAsync(ProjectTaskStatus projectTaskStatus, Guid taskId, Guid projectId);
    Task<bool> DeleteTaskByIdAsync(Guid projectId, Guid taskId);
    Task<ProjectTask> UpdateTaskAsync(Guid projectId, Guid taskId, ProjectTask updatedTask);

}