using System.Net;

namespace mvc_dotnet.Exceptions;


public abstract class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }

    protected AppException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) // if no status code is provided, default to 500 Internal Server Error
        : base(message)
    {
        StatusCode = statusCode;
    }
}