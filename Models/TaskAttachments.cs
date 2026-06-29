using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_dotnet.Models;

public class TaskAttachment : BaseEntity
{

    public Guid TaskId {get;set;} 
    public virtual ProjectTask Task {get;set;} = null!; // navigation property to access task details from attachment
    public string FileName {get;set;} = string.Empty;
    public string FilePath {get;set;} = string.Empty;
    public string FileType {get;set;} = string.Empty;
    public string FileSize {get;set;} = string.Empty;
    public Guid UploadedById {get;set;}  
    public virtual User UploadedBy {get;set;} = null!; // navigation property to access user details from attachment
    public DateTime UploadedAt {get;set;} = DateTime.UtcNow;
   
}