using Consul;
using DistributedKeyValueStore.Node.Config;
using DistributedKeyValueStore.Node.Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedKeyValueStore.Node
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging((builder => builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug).AddConsole()));

            services.AddHttpClient();
            services.AddControllers();

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(config =>
            {
                string serviceDiscovery = Environment.GetEnvironmentVariable("ASPNETCORE_SERVICEDISCOVERY_URL") ?? throw new ConsulConfigurationException("Missing service discovery url");
                string port = Environment.GetEnvironmentVariable("ASPNETCORE_SERVICEDISCOVERY_PORT") ?? throw new ConsulConfigurationException("Missing service discovery port");
                config.Address = new Uri($"http://{serviceDiscovery}:{port}");
            }));

            services.AddSingleton<IHostedService, ConsulHostedService>();
            services.Configure<ConsulConfig>(configuration.GetSection("consul"));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
