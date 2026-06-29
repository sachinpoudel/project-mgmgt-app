using System.ComponentModel.DataAnnotations;
using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.Project;


public class CreateProjectDto

{
    public Guid Id {get;set;} = Guid.NewGuid();
    [Required , StringLength(100, MinimumLength = 3, ErrorMessage = "Project name must be between 3 and 100 characters.")]
    public string ProjectName {get;set;} = string.Empty;
    [Required, StringLength(500 , MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters.")]
    public string Description {get;set;} = string.Empty;
    public DateTime StartDate {get;set;} = DateTime.UtcNow;
    [Required]
    public DateTime EndDate {get;set;}
    public ProjectStatus Status {get;set;} 
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public IEnumerable<Guid> MemberIds {get;set;} = new List<Guid>(); // to add members during project creation
    public Guid OwnerId {get;set;} // to set the project owner during creation

    // public MemberRole Role {get;set;}
}