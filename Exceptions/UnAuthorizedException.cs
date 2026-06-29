namespace mvc_dotnet.Exceptions;

public sealed class UnAuthorizedException : AppException
{
    public UnAuthorizedException(string message): base(message, System.Net.HttpStatusCode.Unauthorized){}
}