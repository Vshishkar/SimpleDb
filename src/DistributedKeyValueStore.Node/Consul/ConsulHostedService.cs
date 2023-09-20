using Consul;
using DistributedKeyValueStore.Node.Config;

namespace DistributedKeyValueStore.Node.Consul
{
    public class ConsulHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IConsulClient _consulClient;
        private readonly ILogger<ConsulHostedService> _logger;

        public string NodeId { get; }
        public string NodeName { get; }


        public ConsulHostedService(IConfiguration configuration, IConsulClient consulClient, ILogger<ConsulHostedService> logger)
        {
            _configuration=configuration;
            _consulClient=consulClient;
            _logger=logger;
            NodeId = $"node-{Guid.NewGuid()}";
            NodeName = NodeId.Substring(0, 10);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Registering service with Consul: {NodeId}");
            // var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>();
            var registration = new AgentServiceRegistration
            {
                ID = NodeId,
                Name = NodeName,
                // Address = serviceConfig.ServiceHost,
                // Port = serviceConfig.ServicePort
            };

            var check = new AgentServiceCheck
            {
                // HTTP = serviceConfig.HealthCheckUrl,
               // Interval = TimeSpan.FromSeconds(serviceConfig.HealthCheckIntervalSeconds),
               // Timeout = TimeSpan.FromSeconds(serviceConfig.HealthCheckTimeoutSeconds)
            };

            // registration.Checks = new[] { check };

            _logger.LogInformation($"Registering service with Consul: {registration.Name}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>();
            var registration = new AgentServiceRegistration { ID = NodeId };

            _logger.LogInformation($"Deregistering service from Consul: {registration.ID}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
        }
    }
}
