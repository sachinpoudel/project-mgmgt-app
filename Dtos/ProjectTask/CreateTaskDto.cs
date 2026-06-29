using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.ProjectTask;


public class CreateTaskDto
{
    public string TaskName {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public ProjectTaskStatus Status {get;set;}
    public TaskPriority Priority {get;set;}
    public int Progress {get;set;}
    public DateTime DueDate {get;set;}
    public decimal EstimatedHours {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public Guid CreatedById {get;set;}
    public Guid ProjectId {get;set;}
    public Guid AssignedToId {get;set;}
}