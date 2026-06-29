using Microsoft.EntityFrameworkCore;
using mvc_dotnet.Data;
using mvc_dotnet.Dtos.Project;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;

namespace mvc_dotnet.Repositories;

public class ProjectRepository(AppDbContext db) : IProjectRepository
{
    public async Task<Project> CreateProjectAsync(Project project, ProjectMember membership, Guid ownerId)
    {
using var transaction = await db.Database.BeginTransactionAsync();

        await db.Projects.AddAsync(project);
        await db.SaveChangesAsync();


await db.ProjectMembers.AddAsync(membership);
        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        return project;
    }

    public async Task<bool> DeleteProjectAsync(Guid projectId)
    {
        var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
       
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExisProjectAsync(Guid projectId)
    {
        return await db.Projects.AnyAsync( p => p.Id == projectId);
   }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        var projects = await db.Projects.ToListAsync();
        return projects;
    }

    public async Task<Project> GetProjectByIdAsync(Guid projectId)
    {
        var project = await db.Projects.Include(p => p.Owner).FirstOrDefaultAsync(p => p.Id == projectId);
      
        return project;
    }

    public Task<int> GetProjectProgressAsync(Guid projectId)
    {
// var project = db.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == projectId);
       
//         var totalTasks = project.Tasks.Count;
//         if (totalTasks == 0) return Task.FromResult(0); // Avoid division by zero
//          var completedTasks = project.Tasks.Count(t => t.Status == "Completed");
//          var progress = (int)((double)completedTasks / totalTasks * 100);
//         return Task.FromResult(progress);
throw new NotImplementedException("GetProjectProgressAsync is not implemented yet.");
    }

    public async Task<IEnumerable<Project>> GetProjectsByOwnerIdAsync(Guid ownerId)
    {
var project = await db.Projects
    .Where(p => p.OwnerId == ownerId).Include(p => p.Owner).Include(p => p.Members).ThenInclude(u => u.User).ToListAsync();
           
        return new List<Project>(project);
    }
  
    

    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId)
    {
        return await db.ProjectMembers.Where(pm => pm.UserId == userId).Select(pm => pm.Project).ToListAsync();
    }

    public async Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId)
    {
      return await db.ProjectMembers.AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
    }

    public async Task<Project> UpdateProjectAsync(Guid projectId, Project updatedProject)
    {
      var projectToUpdate = await db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
      
        projectToUpdate.ProjectName = updatedProject.ProjectName;
        projectToUpdate.Description = updatedProject.Description;
        projectToUpdate.Status = updatedProject.Status;
        projectToUpdate.StartDate = updatedProject.StartDate;
        projectToUpdate.EndDate = updatedProject.EndDate;
        await db.SaveChangesAsync();
        return projectToUpdate;
    }

  
}