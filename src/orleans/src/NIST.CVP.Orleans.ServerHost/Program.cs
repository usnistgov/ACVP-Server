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

namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        private static IServiceProvider ServiceProvider { get; }
        private static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly OrleansConfig OrleansConfig;
        private static readonly EnvironmentConfig EnvironmentConfig;

        static Program()
        {
            ServiceProvider = EntryPointConfigHelper.Bootstrap(RootDirectory);
            OrleansConfig = ServiceProvider.GetService<IOptions<OrleansConfig>>().Value;
            EnvironmentConfig = ServiceProvider.GetService<IOptions<EnvironmentConfig>>().Value;
        }
        
        static void Main(string[] args)
        {
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = OrleansConfig.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .ConfigureServices(ConfigureServices.RegisterServices)
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(IGrainMarker).Assembly).WithReferences();
                })
                .ConfigureClustering(OrleansConfig, EnvironmentConfig)
                .ConfigureLogging(OrleansConfig, EnvironmentConfig)
                .UseDashboard(options => { });

            var silo = builder.Build();
            silo.StartAsync().Wait();

            Console.WriteLine("Press Enter to close.");
            Console.ReadLine();

            // shut the silo down after we are done.
            silo.StopAsync().Wait();
        }
    }
}