using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains;
using Serilog;

namespace NIST.CVP.ACVTS.Orleans.ServerHost
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
                    builder
                        .AddJsonFile($"{directoryConfig}sharedappsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"{directoryConfig}appsettings.json", optional: false, reloadOnChange: false);
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
