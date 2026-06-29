using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvc_dotnet.Dtos.Auth;

public class LoginDto
{

    [Required(ErrorMessage = "Email is Required")]
    public string Email { get; init; } = string.Empty;
    [Required(ErrorMessage = "Password is Required")]
    [StringLength(100, MinimumLength =6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DisplayName("Password")]
    [DataType(DataType.Password)]
    public string Password {get;set;} = string.Empty;

    [DisplayName("Remember Me")]
    public bool RememberMe { get; set; } = false;
}