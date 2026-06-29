using mvc_dotnet.Dtos.Project;
using mvc_dotnet.Models;

namespace mvc_dotnet.Services.Interfaces;

public interface IProjectService
{
 Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync();
    Task<ProjectDetailDto> GetProjectByIdAsync(Guid projectId);
    Task<CreateProjectDto> CreateProjectAsync( CreateProjectDto projectDto,  Guid ownerId);
    Task<UpdateProjectDto> UpdateProjectAsync(Guid projectId ,  UpdateProjectDto updatedProjectDto);
    Task<bool> DeleteProjectAsync(Guid projectId);
    Task<bool> IsProjectExistAsync(Guid projectId);

    //user specific
Task<IEnumerable<ProjectListDto>> GetProjectsByUserIdAsync(Guid userId);

    Task<IEnumerable<ProjectListDto>> GetProjectsByOwnerIdAsync(Guid userId);
    Task<bool> IsUserInProjectAsync(Guid projectId , Guid userId);
    Task<int> GetProjectProgressAsync(Guid projectId);
}