using mvc_dotnet.Dtos.Auth;
using mvc_dotnet.Enums;
namespace mvc_dotnet.Dtos.Project;

public class ProjectDetailDto
{
    public Guid Id {get;set;}
    public string ProjectName {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public DateTime StartDate {get;set;} = DateTime.UtcNow;
    public DateTime EndDate {get;set;} = DateTime.UtcNow;
    public ProjectStatus Status {get;set;} = ProjectStatus.NotStarted;
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public int DaysRemaining {get;set;}
    public int Progress {get;set;}
    public bool IsOverdue {get;set;}
    public int TotalTasks {get;set;}
    public int CompletedTasks {get;set;}
    public UserShortDto Owner {get;set;} = null!; // navigation property to access owner details from project
    public IEnumerable<ProjectMemberShortDto> TeamMembers {get;set;} = new List<ProjectMemberShortDto>();
    public Dictionary<string, int> TaskByStatus {get;set;} = new Dictionary<string, int>(); // to track task count by status
    public Dictionary<string, int> TaskByPriority {get;set;} = new Dictionary<string, int>(); // to track task count by priority
}