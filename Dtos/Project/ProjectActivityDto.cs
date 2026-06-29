using mvc_dotnet.Dtos.Auth;

namespace mvc_dotnet.Dtos.Project;


public class ProjectActivityDto
{
    public Guid  Id {get;set;} // for tracking activity related to a specific project
    public string Description {get;set;} = string.Empty; // description of the activity (e.g., "Task 'Design UI' marked as completed")
    public DateTime Timestamp {get;set;} = DateTime.UtcNow; // when the activity occurred
    public UserShortDto User {get;set;} = null!; // user who performed the activity (navigation property to access user details)
}