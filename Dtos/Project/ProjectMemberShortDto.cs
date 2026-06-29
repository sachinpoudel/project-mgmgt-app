using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.Project;


public class ProjectMemberShortDto


{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public MemberRole Role { get; set; } 
    public Guid  ProjectId { get; set; } 
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

}