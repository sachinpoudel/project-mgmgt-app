namespace mvc_dotnet.Dtos.Project;

public class ProjectMemberDto
{
    public Guid UserId {get;set;}
    public string Email {get;set;} = string.Empty;
    public Guid ProjectId {get;set;} 
    public string Role {get;set;} = string.Empty;

    public DateTime JoinedAt {get;set;} = DateTime.UtcNow;
    public bool IsActive {get;set;} 
    public int AssignedTasksCount {get;set;} // to track number of tasks assigned to this member
    public int CompletedTasksCount {get;set;} // to track number of tasks completed by this member
    public double CompletionRate => AssignedTasksCount > 0 ? (double)CompletedTasksCount / AssignedTasksCount * 100 : 0; // calculate completion rate as percentage
}