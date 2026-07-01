using mvc_dotnet.Models;

namespace mvc_dotnet.Repositories.Interface;


public interface IUserRepository
{
    Task<bool> ExistsUserAsync(Guid userId);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password);
    Task<User?> LoginUserAsync(string email, string password);
    Task<bool> UpdateLastLoginAsync(Guid id);
}