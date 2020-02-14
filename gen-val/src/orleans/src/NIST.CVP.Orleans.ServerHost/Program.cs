using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.ServerHost.Models;
using Serilog;

namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var directoryConfig = EntryPointConfigHelper.GetRootDirectory();
            await CreateHostBuilder(args, directoryConfig).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, string directoryConfig) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    if (string.IsNullOrWhiteSpace(env))
                    {
                        /* TODO this could fall back to an environment,
                         * when/if driver is updated to check for var
                         */
                        throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
                    }

                    context.HostingEnvironment.EnvironmentName = env;
                    
                    builder
                        .AddJsonFile($"{directoryConfig}sharedappsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"{directoryConfig}sharedappsettings.{env}.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"{directoryConfig}appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"{directoryConfig}appsettings.{env}.json", optional: false, reloadOnChange: false);
                })
                .UseWindowsService()
                .UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    ConfigureServices.RegisterServices(hostContext.Configuration, services);
                    services.AddHostedService<OrleansSiloHost>();
                });
    }
}
