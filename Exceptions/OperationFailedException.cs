namespace mvc_dotnet.Exceptions;


public sealed class OperationFailedException : AppException
{
    public OperationFailedException(string message) : base(message, System.Net.HttpStatusCode.InternalServerError){}
}