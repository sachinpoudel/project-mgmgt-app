using System.ComponentModel.DataAnnotations;

namespace mvc_dotnet.Dtos.Project;

public class AddMemberToProjectDto
{
    [Required]
    public Guid UserId { get; set; } 
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Role { get; set; } = string.Empty;
}