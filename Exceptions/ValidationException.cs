namespace mvc_dotnet.Exceptions;


public sealed class ValidationException: AppException
{
    public ValidationException(string message) : base(message,System.Net.HttpStatusCode.BadRequest){}
}