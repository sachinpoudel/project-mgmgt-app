namespace mvc_dotnet.Exceptions;


public sealed class ForbiddenException: AppException
{
    public ForbiddenException(string message) : base(message, System.Net.HttpStatusCode.Forbidden){}
}