using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using mvc_dotnet.Exceptions;
using mvc_dotnet.Models;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using mvc_dotnet.Repositories.Interface;
using mvc_dotnet.Services.Interfaces;
using mvc_dotnet.Repositories;
using mvc_dotnet.Services;
using mvc_dotnet.Data;
using System.Text.Json;
using mvc_dotnet.Extensions;
using mvc_dotnet.Service;
using mvc_dotnet.Middleware;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
        ClockSkew = TimeSpan.Zero // this is to prevent token expiration issues due to clock differences between the server and the client
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse(); // stop default behavior
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                status = 401,
                message = "You are not authorized. Token missing or invalid."
            });

            await context.Response.WriteAsync(result);
        },
        OnAuthenticationFailed = async context =>
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                status = 401,
                message = "Token validation failed: " + context.Exception.Message
            });

            await context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddAuthorization(); // this is required to use the [Authorize] attribute in controllers or endpoints
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services));

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
        ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
        ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();



var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "RedisCacheInstance";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddRateLimiting(builder.Configuration); // Add rate limiting services

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddTransient<RequestLoggingMiddleware>();

builder.Services.AddRepositories(); // Register repositories with caching decorators

builder.Services.AddControllers();
var app = builder.Build();
 app.UseMiddleware<RequestLoggingMiddleware>(); // Custom middleware for logging requests and responses
app.UseExceptionHandler();
app.UseRouting();
app.UseRateLimiter(); // Apply rate limiting middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => "Hello World!");

app.Run();
