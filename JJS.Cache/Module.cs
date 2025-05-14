using JJS.Cache.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JJS.Cache;

public static class Module
{
    public static IServiceCollection UseCacheService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSingleton<CacheService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<CacheService>>();
            var defaultExpiration = TimeSpan.FromSeconds(60);
            try
            {
                defaultExpiration = TimeSpan.FromMilliseconds(
                    configuration.GetValue<int>("Cache:DefaultExpiration")
                );
            }
            catch
            {
                logger.LogError("Could not load Cache:DefaultExpiration");
            }
            return new CacheService(logger, defaultExpiration);
        });

        return services;
    }
}
