namespace mvc_dotnet.Middleware;

public class RequestLoggingMiddleware( ILogger<RequestLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startTime = DateTime.UtcNow;
        var path = context.Request.Path;
        var method = context.Request.Method;
        logger.LogInformation("Received {Method} request for {Path} at {StartTime}", method, path, startTime);
        await next(context); // this calls the next middleware in the pipeline


        var elapsed = DateTime.UtcNow - startTime; // Calculate the time taken to process the request

        var statusCode = context.Response.StatusCode;

        logger.LogInformation("Completed {Method} request for {Path} with status code {StatusCode} in {ElapsedMilliseconds} ms", method, path, statusCode, elapsed.TotalMilliseconds);
    }
}