using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.ServerHost.ExtensionMethods;
using NIST.CVP.Orleans.ServerHost.Models;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace NIST.CVP.Orleans.ServerHost
{
    public class OrleansSiloHost : IHostedService
    {
        private readonly OrleansConfig _orleansConfig;
        private readonly EnvironmentConfig _environmentConfig;
        private ISiloHost _silo;

        public OrleansSiloHost(DirectoryConfig rootDirectory)
        {
            var serviceProvider = EntryPointConfigHelper.Bootstrap(rootDirectory.RootDirectory);
            _orleansConfig = serviceProvider.GetService<IOptions<OrleansConfig>>().Value;
            _environmentConfig = serviceProvider.GetService<IOptions<EnvironmentConfig>>().Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = _orleansConfig.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .ConfigureServices(ConfigureServices.RegisterServices)
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(IGrainMarker).Assembly).WithReferences();
                })
                .ConfigureClustering(_orleansConfig, _environmentConfig)
                .ConfigureLogging(_orleansConfig, _environmentConfig)
                .UseDashboard(options => { }); // port 8080

            _silo = builder.Build();
            await _silo.StartAsync(cancellationToken);
        }
        
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _silo.StopAsync(cancellationToken);
        }
    }
}
