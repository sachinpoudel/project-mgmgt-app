using mvc_dotnet.Middleware;

namespace mvc_dotnet.Extensions;


public static class MiddlewareExtension
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}