using mvc_dotnet.Dtos.ProjectTask;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Services;

public class ProjectTaskService(IProjectTaskRepository projectTaskRepository, IProjectRepository projectRepository, IUserRepository userRepository) : IProjectTaskService

{
    public Task<TaskListDto?> AssignTaskToMemberAsync(Guid projectId, Guid taskId, Guid assigneeId)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateTaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, Guid projectId, Guid userId)
    {
        if (!await userRepository.ExistsUserAsync(userId))
        {
            throw new BadRequestException("Invalid UserId");
        }
        if (!await projectRepository.ExisProjectAsync(projectId))
        {
            throw new BadRequestException("Invalid projectId");
        }
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = createTaskDto.TaskName,
            Description = createTaskDto.Description,
            Status = createTaskDto.Status,
            ProjectId = projectId,
            CreatedById = userId,
            CreatedAt = DateTime.UtcNow
        };
        var createdTask = await projectTaskRepository.CreateTaskAsync(task);
  
        return new CreateTaskDto
        {
           TaskName = createdTask.Title,
           Description = createdTask.Description,
           Status = createdTask.Status,
           ProjectId = createdTask.ProjectId,
           CreatedById = createdTask.CreatedById,
           CreatedAt = createdTask.CreatedAt
        };
        
    }

    public async Task<IEnumerable<ProjectTask>> GetAllTasksByProjectIdAsync(Guid projectId)
    {
        var isProjectIdValid = await projectRepository.ExisProjectAsync(projectId);
        if (!isProjectIdValid)
        {
            throw new BadRequestException("Invalid projectId");
        }
        var tasks = await projectTaskRepository.GetAllTasksByProjectIdAsync(projectId);
        if (tasks is null)
        {
            throw new NotFoundException("No tasks found for the given projectId", projectId.ToString());
        }
        return new List<ProjectTask>(tasks);
    }

    public async Task<TaskListDto> GetSingleTaskByIdAsync(Guid projectId , Guid taskId)
    {
        var isTaskIdValid = await projectTaskRepository.IsTaskExistsAsync(projectId, taskId);
        if(!isTaskIdValid)
        {
            throw new BadRequestException("Invalid taskId");
        }
        var task = await projectTaskRepository.GetSingleTaskByIdAsync(projectId, taskId);
        if(task is null)
        {
            throw new NotFoundException("Task not found", taskId.ToString());
        }
        return new TaskListDto
        {
            Id = task.Id,
            TaskName = task.Title,
            Description = task.Description,
            Status = task.Status,
            ProjectId = task.ProjectId,
            CreatedById = task.CreatedById,
            CreatedAt = task.CreatedAt
        };
    }
}