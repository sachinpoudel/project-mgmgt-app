using mvc_dotnet.Models;

namespace mvc_dotnet.Services.Interfaces;


public interface ITokenService
{
    string CreateToken(User user);
}