namespace mvc_dotnet.Dtos.Project;

public class ProjectStatsDto
{
    public Guid ProjectId { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int Progress { get; set; }
    public int TotalMembers { get; set; }
    public Dictionary<string, int> TasksByPriority { get; set; }
    public Dictionary<string, int> TasksByStatus { get; set; }
    public Dictionary<string, int> TasksByAssignee { get; set; }
}