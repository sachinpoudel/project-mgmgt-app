using System.Security.Claims;
using mvc_dotnet.Exceptions;

namespace  mvc_dotnet.Extensions;


public static class ClaimsPrincipalExtension
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value; // "sub" is the standard claim type for user ID in JWT tokens


        if(string.IsNullOrEmpty(userId) || !Guid.TryParse(userId , out var id))
        {
            throw new UnAuthorizedException("User ID claim is missing or invalid.");
        }
        return id;
    }
}
