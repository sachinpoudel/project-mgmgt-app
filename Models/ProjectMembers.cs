using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mvc_dotnet.Enums;

namespace mvc_dotnet.Models;

public class ProjectMember : BaseEntity
{
    public Guid ProjectId {get;set;} // this auto 
    public virtual Project Project {get;set;} = null!; // navigation property to access project details from member
    public Guid UserId {get;set;}
    public virtual User User {get;set;} = null!; // navigation property to access user details from member
    public MemberRole Role {get;set;} 
    public DateTime JoinedAt {get;set;} = DateTime.UtcNow;
  
   
    public virtual ICollection<ProjectTask> AssignedTasks {get;set;} = new HashSet<ProjectTask>(); // to track tasks assigned to this member
}