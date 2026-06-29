using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mvc_dotnet.Enums;

namespace mvc_dotnet.Models;

public class ProjectTask : BaseEntity
{

public Guid ProjectId {get;set;}
public virtual Project Project {get;set;} = null!; // navigation property to access project details from task
  
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public ProjectTaskStatus Status { get; set; }  
    public TaskPriority Priority { get; set; } 
    public int Progress { get; set; }
    public DateTime DueDate { get; set; }

    public decimal EstimatedHours { get; set; }

    public Guid CreatedById { get; set; }
    public virtual User CreatedBy { get; set; } = null!; // navigation property to access creator details from task
    public Guid? AssignedToId { get; set; }
    public virtual User AssignedTo { get; set; } = null!; // navigation
    public Guid? ParentTaskId { get; set; } // 
    public virtual ProjectTask? ParentTask { get; set; } // navigation property to access parent task details from subtask
    public string Tags { get; set; } = string.Empty;






    public virtual ICollection<ProjectTask> SubTasks {get;set;} = new HashSet<ProjectTask>(); // to track subtasks of this task

    public virtual ICollection<TaskComment> Comments {get;set;} = new HashSet<TaskComment>();
    public virtual ICollection<TaskAttachment> Attachments {get;set;} = new HashSet<TaskAttachment>();
    public virtual ICollection<TimeLog> TimeLogs {get;set;} = new HashSet<TimeLog>();

}