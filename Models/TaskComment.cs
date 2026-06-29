using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_dotnet.Models;


public class TaskComment: BaseEntity
{
    public Guid TaskId {get;set;} 
    public virtual ProjectTask Task {get;set;} = null!; // navigation property to access task details from comment
    public Guid UserId {get;set;}
    public virtual User User {get;set;} = null!; // navigation property to access user details from comment
    public string CommentText {get;set;} = string.Empty;
    public DateTime dateTime    {get;set;} = DateTime.UtcNow;
    
  
}