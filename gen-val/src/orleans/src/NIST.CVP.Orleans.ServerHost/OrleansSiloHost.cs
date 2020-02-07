using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.ServerHost.Models;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Statistics;
using Serilog.Extensions.Logging;
using Environments = NIST.CVP.Common.Enums.Environments;

namespace NIST.CVP.Orleans.ServerHost
{
    public class OrleansSiloHost : IHostedService
    {
        private readonly ILogger<OrleansSiloHost> _logger;
        private readonly IConfiguration _configuration;
        private readonly OrleansConfig _orleansConfig;
        private readonly EnvironmentConfig _environmentConfig;
        private readonly string _connectionString;

        private ISiloHost _silo;
        
        public OrleansSiloHost(
            ILogger<OrleansSiloHost> logger, 
            IConfiguration configuration,
            IDbConnectionStringFactory dbConnectionStringFactory,
            IOptions<OrleansConfig> orleansConfig,
            IOptions<EnvironmentConfig> environmentConfig)
        {
            _logger = logger;
            _logger.LogInformation("Orleans Silo initializing.");

            _configuration = configuration;
            _connectionString = dbConnectionStringFactory.GetConnectionString(Constants.OrleansConnectionString);
            _orleansConfig = orleansConfig.Value;
            _environmentConfig = environmentConfig.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = _orleansConfig.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .Configure<GrainCollectionOptions>(options =>
                {
                    options.CollectionAge = TimeSpan.FromMinutes(5);
                })
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(IGrainMarker).Assembly).WithReferences();
                })
                .ConfigureServices(svcCollection =>
                {
                    ConfigureServices.RegisterServices(_configuration, svcCollection);
                })
                .UsePerfCounterEnvironmentStatistics()
                .UseDashboard(options =>
                {
                    options.Port = _orleansConfig.OrleansDashboardPort;
                    options.CounterUpdateIntervalMs = 10000;
                });

            ConfigureClustering(builder);
            ConfigureLoadShedding(builder);
            ConfigureLogging(builder);

            _silo = builder.Build();
            await _silo.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _silo.StopAsync(cancellationToken);
        }

        private void ConfigureClustering(ISiloHostBuilder builder)
        {
            switch (_environmentConfig.Name)
            {
                case Environments.Local:
                    builder.Configure<EndpointOptions>(options =>
                    {
                        options.AdvertisedIPAddress = IPAddress.Loopback;
                    });
                    builder.UseLocalhostClustering();
                    break;
                case Environments.Tc:
                    var primarySiloEndpoint = new IPEndPoint(
                        IPAddress.Parse(_orleansConfig.OrleansNodeConfig.First().HostName),
                        _orleansConfig.OrleansSiloPort
                    );
                    builder.UseDevelopmentClustering(primarySiloEndpoint);
                    builder.ConfigureEndpoints(
                        siloPort: _orleansConfig.OrleansSiloPort,
                        gatewayPort: _orleansConfig.OrleansGatewayPort
                    );
                    break;
                default:
                    builder.UseAdoNetClustering(options =>
                    {
                        options.Invariant = "System.Data.SqlClient";
                        options.ConnectionString = _connectionString;
                    });
                    builder.ConfigureEndpoints(
                        siloPort: _orleansConfig.OrleansSiloPort,
                        gatewayPort: _orleansConfig.OrleansGatewayPort
                    );
                    break;
            }
        }

        private void ConfigureLoadShedding(ISiloHostBuilder builder)
        {
            if (_orleansConfig.LoadSheddingCpuThreshold > 0)
            {
                builder.Configure<LoadSheddingOptions>(options =>
                {
                    options.LoadSheddingEnabled = true;
                    options.LoadSheddingLimit = _orleansConfig.LoadSheddingCpuThreshold;
                });
            }
        }

        private void ConfigureLogging(ISiloHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(_orleansConfig.MinimumLogLevel);

                if (_orleansConfig.UseConsoleLogging)
                {
                    logging.AddConsole();
                }

                if (_orleansConfig.UseFileLogging)
                {
                    logging.AddProvider(new SerilogLoggerProvider());                    
                }
            });
        }
    }
}
