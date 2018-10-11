using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.ServerHost.ExtensionMethods;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace NIST.CVP.Orleans.ServerHost
{
    public class OrleansSiloHost
    {
        private readonly OrleansConfig _orleansConfig;
        private readonly EnvironmentConfig _environmentConfig;
        private ISiloHost _silo;

        public OrleansSiloHost(string rootDirectory)
        {
            var serviceProvider = EntryPointConfigHelper.Bootstrap(rootDirectory);
            _orleansConfig = serviceProvider.GetService<IOptions<OrleansConfig>>().Value;
            _environmentConfig = serviceProvider.GetService<IOptions<EnvironmentConfig>>().Value;
        }

        public void StartSilo()
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
                .UseDashboard(options => { });

            _silo = builder.Build();
            _silo.StartAsync().FireAndForget();
        }

        public void StopSilo()
        {
            _silo.StopAsync().FireAndForget();
        }
    }
}
