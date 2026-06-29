using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using mvc_dotnet.Models;
using mvc_dotnet.Services.Interfaces;

namespace mvc_dotnet.Services;


public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(User user)
    {
        
        var claims = new List<Claim>
       {
           new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
           new Claim(JwtRegisteredClaimNames.UniqueName, user.FirstName + " " + user.LastName),
           new Claim(JwtRegisteredClaimNames.Email, user.Email),
           new Claim(ClaimTypes.Role, user.Role.ToString())
       };
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = credentials,
            Issuer = config["JWT:Issuer"],
            Audience = config["JWT:Audience"]
        };
        var handler = new JsonWebTokenHandler(); // Use JsonWebTokenHandler instead of JwtSecurityTokenHandler bcz it is more efficient and provides better performance for creating and validating JWTs.
        var token = handler.CreateToken(descriptor);
        return token;
    }
}