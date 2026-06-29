using mvc_dotnet.Dtos.Project;
using mvc_dotnet.Models;

namespace mvc_dotnet.Repositories.Interface;

public interface IProjectRepository
{
    Task<bool> ExisProjectAsync(Guid projectId);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(Guid projectId);
    Task<Project> CreateProjectAsync( Project project, ProjectMember membership, Guid ownerId);
    Task<Project> UpdateProjectAsync(Guid projectId ,  Project updatedProject);
    Task<bool> DeleteProjectAsync(Guid projectId);
    Task<IEnumerable<Project>> GetProjectsByOwnerIdAsync(Guid ownerId);

    Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId);
    Task<int> GetProjectProgressAsync(Guid projectId);
    Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId);
   
}