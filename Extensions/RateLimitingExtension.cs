using Microsoft.AspNetCore.RateLimiting;
using RedisRateLimiting.AspNetCore;
using StackExchange.Redis;

public static class RateLimitingExtension
{
    public static IServiceCollection AddRateLimiting(
        this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Must be first and outside — registers required services
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // 2. Then configure policies with IConnectionMultiplexer injected
        services.PostConfigure<RateLimiterOptions>((options) =>
                {
                    var redis = services.BuildServiceProvider().GetRequiredService<IConnectionMultiplexer>();
                    
                    options.AddRedisFixedWindowLimiter("internal-rpc", opt =>
                    {
                        opt.ConnectionMultiplexerFactory = () => redis;
                        opt.PermitLimit = configuration.GetValue<int>("RateLimiting:internal-rpc:PermitLimit");
                        opt.Window = TimeSpan.FromSeconds(
                            configuration.GetValue<int>("RateLimiting:internal-rpc:WindowInSeconds"));
                    });

                    options.AddRedisTokenBucketLimiter("public-api", opt =>
                    {
                        opt.ConnectionMultiplexerFactory = () => redis;
                        opt.TokenLimit = configuration.GetValue<int>("RateLimiting:public-api:TokenLimit");
                        opt.TokensPerPeriod = configuration.GetValue<int>("RateLimiting:public-api:TokensPerPeriod");
                        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(
                            configuration.GetValue<int>("RateLimiting:public-api:ReplenishmentPeriodInSeconds"));
                    });

                    options.AddRedisSlidingWindowLimiter("auth-endpoints", opt =>
                    {
                        opt.ConnectionMultiplexerFactory = () => redis;
                        opt.PermitLimit = configuration.GetValue<int>("RateLimiting:auth-endpoints:PermitLimit");
                        opt.Window = TimeSpan.FromSeconds(
                            configuration.GetValue<int>("RateLimiting:auth-endpoints:WindowInSeconds"));
                    });
                });

        return services;
    }
}