using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mvc_dotnet.Enums;

namespace mvc_dotnet.Models;

public class Project : BaseEntity
{
 // construcor is not need if direct initialization is used for collections
    public string ProjectName {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    
    public DateTime StartDate {get;set;} = DateTime.UtcNow;
    public DateTime EndDate {get;set;} = DateTime.UtcNow;

    public ProjectStatus Status {get;set;} 
    public Guid OwnerId {get;set;}
    public virtual User Owner {get;set;} = null!; // navigation property to access owner details from project
   

    public virtual ICollection<ProjectTask> Tasks {get;set;} = new HashSet<ProjectTask>();
    public virtual ICollection<ProjectMember> Members {get;set;} = new HashSet<ProjectMember>();
    // collection is on many side of one to many relationship, so it is initialized to avoid null reference exception when adding members to project
}