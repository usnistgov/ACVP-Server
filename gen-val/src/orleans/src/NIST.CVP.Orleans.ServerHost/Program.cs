using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Orleans.ServerHost.Models;
using Serilog;

namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var directoryConfig = GetDirectoryConfig();
            
            await CreateHostBuilder(args, directoryConfig).Build().RunAsync();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args, DirectoryConfig directoryConfig) => 
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddSingleton(s => directoryConfig);
                    services.AddHostedService<OrleansSiloHost>();
                });
        
        private static DirectoryConfig GetDirectoryConfig()
        {
            var executingLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var rootDirectory = Path.GetDirectoryName(executingLocation) + "\\";
            Console.WriteLine($"{nameof(rootDirectory)}: {rootDirectory}");

            return new DirectoryConfig()
            {
                RootDirectory = rootDirectory
            };
        }
    }
}
