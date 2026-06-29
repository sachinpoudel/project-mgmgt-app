using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_dotnet.Models;

public class TimeLog : BaseEntity
{
    public Guid TaskId { get; set; }
    public virtual ProjectTask Task { get; set; } = null!; // navigation property to access task details from time log
    public Guid UserId { get; set; }  
    public virtual User User { get; set; } = null!; // navigation property to access user details from time log
    public DateTime LogDate { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;

    
    public decimal HoursSpent { get; set; }

  
}