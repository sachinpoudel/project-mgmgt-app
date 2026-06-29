using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Dtos.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ILogger<AuthController> logger) : ControllerBase
{
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {

      logger.LogInformation("Registering user with email: {Email}", registerDto.Email); 
      logger.LogInformation("Registering user with first name: {FirstName}", registerDto.FirstName);
      logger.LogInformation("Registering user with last name: {LastName}", registerDto.LastName);
        var user = await userService.RegisterUserAsync(registerDto);
          return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
    }




    [HttpPost("login")]
[EnableRateLimiting("auth-endpoints")] // Apply rate limiting to the login endpoint
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await userService.LoginUserAsync(loginDto);
        return Ok(user);
    }
    [HttpGet("users")]
    [Authorize]
    [EnableRateLimiting("public-api")] // Apply rate limiting to the get all users endpoint
    public async Task<IActionResult> GetAllUsers()
    {
        logger.LogInformation("Fetching all users from the service.");
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUsersById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return Ok(user);
    }
    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Global handler test");
    }
   
}