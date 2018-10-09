using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.ServerHost.ExtensionMethods;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.ServiceProcess;

namespace NIST.CVP.Orleans.ServerHost
{
    public class OrleansServerService : ServiceBase
    {
        private static IServiceProvider ServiceProvider { get; }
        private static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly OrleansConfig OrleansConfig;
        private static readonly EnvironmentConfig EnvironmentConfig;
        private static ISiloHost _silo;

        static OrleansServerService()
        {
            ServiceProvider = EntryPointConfigHelper.Bootstrap(RootDirectory);
            OrleansConfig = ServiceProvider.GetService<IOptions<OrleansConfig>>().Value;
            EnvironmentConfig = ServiceProvider.GetService<IOptions<EnvironmentConfig>>().Value;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = OrleansConfig.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .ConfigureServices(Grains.ConfigureServices.RegisterServices)
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(IGrainMarker).Assembly).WithReferences();
                })
                .ConfigureClustering(OrleansConfig, EnvironmentConfig)
                .ConfigureLogging(OrleansConfig, EnvironmentConfig)
                .UseDashboard(options => { });

            _silo = builder.Build();
            _silo.StartAsync().Wait();
        }

        protected override void OnStop()
        {
            _silo.StopAsync().Wait();
        }
    }
}
