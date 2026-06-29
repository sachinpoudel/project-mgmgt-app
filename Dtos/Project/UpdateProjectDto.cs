using System.ComponentModel.DataAnnotations;
using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.Project;


public class UpdateProjectDto
{
    [Required]
    public Guid ProjectId {get;set;}
    public string Name {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public DateTime StartDate {get;set;} = DateTime.UtcNow;
    public DateTime EndDate {get;set;} = DateTime.UtcNow;
    public ProjectStatus Status {get;set;} = ProjectStatus.NotStarted;
    public Guid OwnerId {get;set;}
}