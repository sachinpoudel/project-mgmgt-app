using System.ComponentModel.DataAnnotations;
using mvc_dotnet.Enums;

namespace mvc_dotnet.Models;

public class User: BaseEntity

{
    public User()
    {
        ProjectTasks = new HashSet<ProjectTask>();
        AssignedTasks = new HashSet<ProjectTask>();
        TaskComments = new HashSet<TaskComment>();
        TaskAttachments = new HashSet<TaskAttachment>();
        TimeLogs = new HashSet<TimeLog>();
    }
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
   
    public string Email {get;set;} = string.Empty;

    public byte [] PasswordHash {get;set;} = Array.Empty<byte>();
    public byte[] PasswordSalt {get;set;} = Array.Empty<byte>();
    public string ProfileUrl {get;set;} = string.Empty;
   
    public bool IsActive {get;set;} = true;
  
    public string Role {get;set;} = UserRole.User.ToString();
    public DateTime LastLogin {get;set;} = DateTime.UtcNow;


    public virtual ICollection<ProjectMember> ProjectMembers {get;set;}  = new HashSet<ProjectMember>();// navigation property to access project member details from user


    public virtual ICollection<ProjectTask> ProjectTasks {get;set;}
    public virtual ICollection<ProjectTask> AssignedTasks {get;set;} // to track tasks assigned to this user
   
    public virtual ICollection<TaskComment> TaskComments {get;set;}
    public virtual ICollection<TaskAttachment> TaskAttachments {get;set;}
    public virtual ICollection<TimeLog> TimeLogs {get;set;}
}