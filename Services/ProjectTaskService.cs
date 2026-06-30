using mvc_dotnet.Dtos.Project;
using mvc_dotnet.Dtos.ProjectTask;
using mvc_dotnet.Enums;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Services;

public class ProjectTaskService(IProjectTaskRepository projectTaskRepository, IMemberRepository memberRepository, IProjectRepository projectRepository, IUserRepository userRepository) : IProjectTaskService

{
    public async Task<TaskListDto?> AssignTaskToMemberAsync(Guid projectId, Guid taskId, Guid assigneeId)
    {
        if (!await projectRepository.ExisProjectAsync(projectId))
        {
            throw new BadRequestException("Invalid projectId");
        }
        if (!await userRepository.ExistsUserAsync(assigneeId))
        {
            throw new BadRequestException("Invalid assigneeId");
        }
        if (!await projectTaskRepository.IsTaskExistsAsync(projectId, taskId))
        {
            throw new BadRequestException("Invalid taskId");
        }


        var task = await projectTaskRepository.AssignTaskToMemberAsync(projectId, taskId, assigneeId);

        if (task is null)
        {
            throw new OperationFailedException("Failed to assign task to member");
        }

        return new TaskListDto
        {
            Id = task.Id,
            TaskName = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Progress = task.Progress,
            DueDate = task.DueDate,
            EstimatedHours = task.EstimatedHours,
            CreatedById = task.CreatedById,
            CreatedAt = task.CreatedAt,
            AssignedToId = task.AssignedToId ?? Guid.Empty,
        };
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

    public async Task<bool> DeleteTaskByIdAsync(Guid projectId, Guid taskId, Guid userId)
    {
        var checkIsOwner = await memberRepository.CheckUserRoleInProjectAsync(userId, projectId);
        if (checkIsOwner is null)
        {
            throw new OperationFailedException("User is not a member of the project");
        }
        if (checkIsOwner.Role != MemberRole.Owner)
        {
            throw new UnAuthorizedException("You are not authorized to delete this task");
        }
        var task = await projectTaskRepository.DeleteTaskByIdAsync(projectId, taskId);
        return task;
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

    public async Task<TaskListDto> GetSingleTaskByIdAsync(Guid projectId, Guid taskId)
    {
        var isTaskIdValid = await projectTaskRepository.IsTaskExistsAsync(projectId, taskId);
        if (!isTaskIdValid)
        {
            throw new BadRequestException("Invalid taskId");
        }
        var task = await projectTaskRepository.GetSingleTaskByIdAsync(projectId, taskId);
        if (task is null)
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

    public async Task<UpdateTaskStatusDto> UpdateStatusOfTaskAsync(UpdateProjectTaskDto updateProjectTaskDto, Guid taskId, Guid projectId)
    {
        if (!await projectRepository.ExisProjectAsync(projectId))
        {
            throw new BadRequestException("invalid projectId");
        }
        if (!await projectTaskRepository.IsTaskExistsAsync(projectId, taskId))
        {
            throw new BadRequestException("invalid taskId");
        }
        var taskStatus = await projectTaskRepository.UpdateStatusOfTaskAsync(updateProjectTaskDto.Status, taskId, projectId);
        return new UpdateTaskStatusDto { Status = taskStatus.Status };
    }

    public async Task<UpdateProjectTaskDto> UpdateTaskAsync(Guid projectId, Guid taskId, UpdateProjectTaskDto updateProjectTaskDto)
    {
        var updatedTask = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = updateProjectTaskDto.Title,
            Description = updateProjectTaskDto.Description,


            Status = updateProjectTaskDto.Status,
            Progress = updateProjectTaskDto.Progress

        };
        var task = await projectTaskRepository.UpdateTaskAsync(projectId, taskId, updatedTask);
        if (task is null) {
            throw new OperationFailedException("operaiton failed");
        };
        return new UpdateProjectTaskDto {

            Title = task.Title,
            Description = task.Description,


            Status = task.Status,
            Progress = task.Progress

        };
    }
}
