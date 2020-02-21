using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Helpers;
using Serilog;

namespace Web.Admin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var directoryConfig = EntryPointConfigHelper.GetRootDirectory();
            await CreateHostBuilder(args, directoryConfig).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string directoryConfig) =>
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
                .ConfigureServices((context, collection) =>
                {
                    collection.RegisterAcvpAdminServices();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    }
}