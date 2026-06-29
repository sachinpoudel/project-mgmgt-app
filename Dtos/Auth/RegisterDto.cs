using System.ComponentModel.DataAnnotations;

namespace mvc_dotnet.Dtos.Auth;

public class RegisterDto
{

    [Required(ErrorMessage = "FirstName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "FirstName must be between 2 and 100 chars")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "LastName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "LastName must be between 2 and 100 chars")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}