using mvc_dotnet.Dtos.Auth;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Services;

public class UserService(IUserRepository userRepository, ITokenService tokenService) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllUsersAsync();
        var now = DateTime.UtcNow;

        if (users is null || !users.Any()) throw new NotFoundException("No users found", now);

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Role = u.Role,
            IsActive = u.IsActive
        });
    }


    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        var now = DateTime.UtcNow;
        if (user is null) throw new NotFoundException("User not found", id.ToString());

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin,
            IsActive = user.IsActive
        };
    }

    public async Task<IEnumerable<UserShortDto>> GetUserShorts()
    {
        var users = await userRepository.GetAllUsersAsync();

        if (users is null || !users.Any()) throw new NotFoundException("No users found", DateTime.UtcNow);

        return users.Select(u => new UserShortDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            ProfileUrl = u.ProfileUrl
        });

    }

    public async Task<UserDto> LoginUserAsync(LoginDto loginDto)
    {
        var user = await userRepository.LoginUserAsync(loginDto.Email, loginDto.Password);
        var now = DateTime.UtcNow;
        if (user is null) throw new NotFoundException("Invalid credentials", loginDto.Email);


        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin,
            IsActive = user.IsActive,
            Token = tokenService.CreateToken(user)
        };

    }

    public async Task<UserDto> RegisterUserAsync(RegisterDto registerDto)
    {
 var existingUser = await userRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser is not null) throw new ConflictException("User with this email already exists");

        var user = await userRepository.RegisterUserAsync(registerDto.FirstName, registerDto.LastName, registerDto.Email, registerDto.Password);
        if (user is null) throw new BadRequestException("User registration failed");



        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin,
            IsActive = user.IsActive,
            Token = tokenService.CreateToken(user)
        };
    }

    public async Task<bool> UpdateLastLoginAsync(Guid userId)
    {
        var result = await userRepository.UpdateLastLoginAsync(userId);
        if (!result) throw new NotFoundException("User not found", userId.ToString());
        return result;
    }


    public string GetRelativeTimeString(DateTime pastTime, DateTime currentTime) // this method is used to convert the last login time to a relative time string like "5 minutes ago", "2 hours ago", etc.
    {
        var timeSpan = currentTime - pastTime;

        if (timeSpan.TotalSeconds < 60)
            return $"{(int)timeSpan.TotalSeconds} seconds ago";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minutes ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hours ago";
        if (timeSpan.TotalDays < 30)
            return $"{(int)timeSpan.TotalDays} days ago";
        if (timeSpan.TotalDays < 365)
            return $"{(int)(timeSpan.TotalDays / 30)} months ago";

        return $"{(int)(timeSpan.TotalDays / 365)} years ago";
    }

    public async Task<bool> IsUserExistAsync(Guid userId)
    {
        var result = await userRepository.ExistsUserAsync(userId);
        if (!result) throw new NotFoundException("User not found", userId.ToString());
        return result;
    }
}