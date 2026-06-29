using mvc_dotnet.Dtos.Auth;
using mvc_dotnet.Dtos.Project;
using mvc_dotnet.Enums;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories;
using mvc_dotnet.Repositories.Interface;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Services;


public class ProjectService(IProjectRepository projectRepository, IUserRepository userRepository) : IProjectService
{
    public async Task<CreateProjectDto> CreateProjectAsync(CreateProjectDto projectDto, Guid ownerId)
    {
        var owner = await userRepository.GetUserByIdAsync(ownerId);
        if (owner is null) throw new NotFoundException("Owner not found", ownerId.ToString());


        var project = new Project
        {
            Id = Guid.NewGuid(),
            ProjectName = projectDto.ProjectName,
            Description = projectDto.Description,
            OwnerId = ownerId,
            Status = projectDto.Status,
            CreatedAt = DateTime.UtcNow,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate
        };
        var membership = new ProjectMember
        {
            ProjectId = project.Id,
            UserId = ownerId,
            Role = MemberRole.Owner,
            JoinedAt = DateTime.UtcNow
        };
        var result = await projectRepository.CreateProjectAsync(project, membership, ownerId);
        if (result is null) throw new NotFoundException("Project creation failed", projectDto.ProjectName);

        return new CreateProjectDto
        {
            ProjectName = project.ProjectName,
            Description = project.Description,
            OwnerId = project.OwnerId,
            CreatedAt = DateTime.UtcNow,
            EndDate = project.EndDate,
            StartDate = project.StartDate,
            Status = project.Status,
            MemberIds = project.Members.Select(m => m.UserId).ToList()
        };

    }



    public async Task<bool> DeleteProjectAsync(Guid projectId)
    {
        var project = await projectRepository.DeleteProjectAsync(projectId);
        if (!project) throw new NotFoundException("Project not found", projectId.ToString());
        return true;
    }

    public async Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync()
    {
        var projects = await projectRepository.GetAllProjectsAsync();
        if (projects is null || !projects.Any()) throw new NotFoundException("No projects found", "All Projects");
        return projects.Select(p => new ProjectListDto
        {
            Id = p.Id,
            ProjectName = p.ProjectName,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            Status = p.Status,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            TeamMembers = p.Members.Select(m => new ProjectMemberShortDto
            {
                Id = m.Id,
                UserId = m.UserId,
                FirstName = m.User.FirstName,
                LastName = m.User.LastName,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId


            })
        });
    }

    public async Task<ProjectDetailDto> GetProjectByIdAsync(Guid projectId)
    {
        var project = await projectRepository.GetProjectByIdAsync(projectId);
        if (project is null) throw new NotFoundException("Project not found", projectId.ToString());
        var progress = await projectRepository.GetProjectProgressAsync(projectId);

        if (progress < 0) throw new NotFoundException("Project not found or no tasks associated with the project", projectId.ToString());

        return new ProjectDetailDto
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            Status = project.Status,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Progress = progress,
            TotalTasks = project.Tasks.Count(),
            // CompletedTasks = project.Tasks.Count(t => t.Status == "Completed"),
            Owner = new UserShortDto
            {
                Id = project.Owner.Id,
                FirstName = project.Owner.FirstName,
                LastName = project.Owner.LastName
            },
            TeamMembers = project.Members.Select(m => new ProjectMemberShortDto
            {
                Id = m.Id,
                UserId = m.UserId,
                FirstName = m.User.FirstName,
                LastName = m.User.LastName,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId
            })
        };
    }

    public async Task<int> GetProjectProgressAsync(Guid projectId)
    {
        if (projectId == Guid.Empty) throw new NotFoundException("Project ID is required", projectId.ToString());
        var progress = await projectRepository.GetProjectProgressAsync(projectId);
        if (progress < 0) throw new NotFoundException("Project not found or no tasks associated with the project", projectId.ToString());
        return progress;

    }

    public async Task<IEnumerable<ProjectListDto>> GetProjectsByOwnerIdAsync(Guid OwnerId)
    {
        if (OwnerId == Guid.Empty) throw new NotFoundException("Owner ID is required", OwnerId.ToString());
        var project = await projectRepository.GetProjectsByOwnerIdAsync(OwnerId);
        if (project is null) throw new NotFoundException("No projects found for the given owner", OwnerId.ToString());
        return project.Select(p => new ProjectListDto
        {
            Id = p.Id,
            ProjectName = p.ProjectName,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            Status = p.Status,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            // Progress = p.Tasks.Count() == 0 ? 0 : (int)((double)p.Tasks.Count(t => t.Status == "Completed") / p.Tasks.Count() * 100),
            // TotalTasks = p.Tasks.Count(),
            // CompletedTasks = p.Tasks.Count(t => t.Status == "Completed"),
            Owner = new UserShortDto
            {
                Id = p.OwnerId,
                FirstName = p.Owner.FirstName,
                LastName = p.Owner.LastName


            },
            TeamMembers = p.Members.Select(m => new ProjectMemberShortDto
            {   
                // Id = m.Id,
                UserId = m.UserId,
                // FirstName = m.User.FirstName,
                // LastName = m.User.LastName,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId
            })
        });

    }

    public async Task<IEnumerable<ProjectListDto>> GetProjectsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User ID is required", userId.ToString());
        var projects = await projectRepository.GetProjectsByUserIdAsync(userId);
        if (projects is null || !projects.Any()) throw new NotFoundException("No projects found for the given user", userId.ToString());
        return projects.Select(p => new ProjectListDto
        {
            Id = p.Id,
            ProjectName = p.ProjectName,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            Status = p.Status,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Owner = new UserShortDto
            {
                Id = p.OwnerId,
            },
        });
    }

    public async Task<bool> IsProjectExistAsync(Guid projectId)
    {
        if (projectId == Guid.Empty) throw new NotFoundException("Project ID is required", projectId.ToString());
        var exists = await projectRepository.ExisProjectAsync(projectId);
        if (!exists) throw new NotFoundException("Project not found", projectId.ToString());
        return exists;
    }

    public async Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId)
    {
        if (projectId == Guid.Empty || userId == Guid.Empty) throw new NotFoundException("Project ID and User ID are required", $"{projectId} - {userId}");
        var result = await projectRepository.IsUserInProjectAsync(projectId, userId);
        if (result == false) throw new NotFoundException("User is not a member of the project", $"{projectId} - {userId}");
        return result;
    }

    public async Task<UpdateProjectDto> UpdateProjectAsync(Guid projectId, UpdateProjectDto updatedProjectDto)
    {
        var UpdatedProject = new Project
        {
            ProjectName = updatedProjectDto.Name,
            Description = updatedProjectDto.Description,
            StartDate = updatedProjectDto.StartDate,
            EndDate = updatedProjectDto.EndDate,
            Status = updatedProjectDto.Status,
            OwnerId = updatedProjectDto.OwnerId

        };

        var result = await projectRepository.UpdateProjectAsync(projectId, UpdatedProject);
        if (result is null) throw new NotFoundException("Project update failed", projectId.ToString());
        return new UpdateProjectDto
        {
            ProjectId = result.Id,
            Name = result.ProjectName,
            Description = result.Description,
            StartDate = result.StartDate,
            EndDate = result.EndDate,
            Status = result.Status,
            OwnerId = result.OwnerId
        };
    }

}