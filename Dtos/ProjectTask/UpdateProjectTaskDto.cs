using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.ProjectTask
{
    public class UpdateProjectTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProjectTaskStatus Status { get; set; }
        public int Progress { get; set; }
    }
}