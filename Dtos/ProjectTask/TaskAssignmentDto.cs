using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.ProjectTask;


public class TaskAssignmentDto
{
    public Guid TaskId { get; set; }
    public Guid AssignedToId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public int Progress { get; set; }
    public DateTime DueDate { get; set; }


}