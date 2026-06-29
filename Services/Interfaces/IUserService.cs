using mvc_dotnet.Dtos.Auth;

namespace mvc_dotnet.Services.Interfaces;


public interface IUserService 
{
    Task<bool> IsUserExistAsync(Guid userId);
    Task<UserDto> RegisterUserAsync(RegisterDto registerDto);
    Task<UserDto> LoginUserAsync(LoginDto loginDto);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<bool> UpdateLastLoginAsync(Guid userId);
    Task<IEnumerable<UserShortDto>> GetUserShorts();
}