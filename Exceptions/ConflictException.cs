using System.Net;

namespace mvc_dotnet.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message, HttpStatusCode.Conflict)
    {
    }
}