using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Orleans.Grains;
using Serilog;

namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
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
                    
                    builder.AddJsonFile($"sharedappsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"sharedappsettings.{env}.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false);
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
