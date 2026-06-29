using Microsoft.Extensions.Caching.Distributed;
using mvc_dotnet.Repositories;
using mvc_dotnet.Repositories.Cache;
using mvc_dotnet.Repositories.Interface;

namespace mvc_dotnet.Extensions;


public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<MemberRepository>();
        services.AddScoped<ProjectRepository>();
        services.AddScoped<UserRepository>();


        services.AddScoped<IMemberRepository>(sp =>
    new MemberCacheRepository(
        sp.GetRequiredService<MemberRepository>(),
        sp.GetRequiredService<IDistributedCache>(),
        sp.GetRequiredService<ILogger<MemberCacheRepository>>()
    ));


        services.AddScoped<IProjectRepository>(sp =>
        {
            return new ProjectCacheRepository(
                sp.GetRequiredService<IDistributedCache>(),
                sp.GetRequiredService<ILogger<ProjectCacheRepository>>(),
                sp.GetRequiredService<ProjectRepository>()
            );
        });
        services.AddScoped<IUserRepository>(sp =>
        new UserCacheRepository(
            sp.GetRequiredService<IDistributedCache>(),
            sp.GetRequiredService<UserRepository>(),
            sp.GetRequiredService<ILogger<UserCacheRepository>>()
        ));
        return services;
    }
}