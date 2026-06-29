using Microsoft.EntityFrameworkCore;
using mvc_dotnet.Data;
using mvc_dotnet.Enums;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;

namespace mvc_dotnet.Repositories;


public class ProjectTaskRepository(AppDbContext db) : IProjectTaskRepository
{
    public async Task<ProjectTask?> AssignTaskToMemberAsync(Guid projectId, Guid taskId, Guid assigneeId)
    {
        var task = await db.ProjectTasks.Where(pt => pt.Id == taskId && pt.ProjectId == projectId).FirstOrDefaultAsync();
        task.AssignedToId = assigneeId;
        await db.SaveChangesAsync();
        return task;
    }

    public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
    {
        await db.ProjectTasks.AddAsync(task);
        await db.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTaskByIdAsync(Guid projectId, Guid taskId)
    {
        var task = await db.ProjectTasks.Where(pt => pt.ProjectId == projectId && pt.Id == taskId).FirstOrDefaultAsync();
         db.ProjectTasks.Remove(task);
         await db.SaveChangesAsync();
         return true;
    }

    public async Task<IEnumerable<ProjectTask>> GetAllTasksByProjectIdAsync(Guid projectId)

    {
        return await db.ProjectTasks.Where(pt => pt.ProjectId == projectId).ToListAsync();
    }

    public async Task<ProjectTask?> GetSingleTaskByIdAsync(Guid projectId, Guid taskId)
    {
        return await db.ProjectTasks.FirstOrDefaultAsync(pt => pt.Id == taskId && pt.ProjectId == projectId);
        
    }

    public Task<bool> IsTaskExistsAsync(Guid projectId, Guid taskId)
    {
       return db.ProjectTasks.AnyAsync(pt => pt.Id == taskId && pt.ProjectId == projectId);
    }

    public async Task<ProjectTask?> UpdateStatusOfTaskAsync(ProjectTaskStatus projectTaskStatus, Guid taskId, Guid projectId)
    {
        var task = await db.ProjectTasks.Where(pt => pt.Id == taskId && pt.ProjectId == projectId).FirstOrDefaultAsync();
        task.Status = projectTaskStatus;
        await db.SaveChangesAsync();
        return task;
    }

    public async Task<ProjectTask> UpdateTaskAsync(Guid projectId, Guid taskId, ProjectTask updatedTask)
    {
       var task = await db.ProjectTasks.Where(pt => pt.ProjectId == projectId && pt.Id == taskId).FirstOrDefaultAsync();
       task.Title = updatedTask.Title;
       task.Description = updatedTask.Description;
       task.Status = updatedTask.Status;
       task.AssignedToId = updatedTask.AssignedToId;
       task.DueDate = updatedTask.DueDate; 

       await db.SaveChangesAsync();
       return task;
    }
}