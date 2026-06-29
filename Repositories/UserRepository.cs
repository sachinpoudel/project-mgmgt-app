using Microsoft.EntityFrameworkCore;
using mvc_dotnet.Data;
using mvc_dotnet.Models;
using mvc_dotnet.Repositories.Interface;
using System.Security.Cryptography;
using System.Text;
namespace mvc_dotnet.Repositories;

public class UserRepository(AppDbContext db, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        logger.LogInformation("Fetching all users from the database.");
        return await db.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await db.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
         var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        logger.LogInformation($"users", user);
        return user;
    }


    public async Task<User> LoginUserAsync(string email, string password)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) return null;

        var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        if (!computedHash.SequenceEqual(user.PasswordHash)) return null;

        return user;
    }

    public async Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password)
    {
        var hmac = new HMACSHA512();
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var passwordSalt = hmac.Key;

        

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            IsActive = true,
            // LastLoginFormatted = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };
        await db.Users.AddAsync(newUser);
        await db.SaveChangesAsync();
        return newUser;

    }

    public async Task<bool> UpdateLastLoginAsync(Guid id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return false;


        user.LastLogin = DateTime.UtcNow;
        db.Users.Update(user);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsUserAsync(Guid userId)
    {
        return await db.Users.AnyAsync( u => u.Id == userId);
    }
}