using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Orleans.ServerHost.ExtensionMethods;
using NIST.CVP.Orleans.ServerHost.Models;

namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        private static bool _isService;

        public static async Task Main(string[] args)
        {
            _isService = !(Debugger.IsAttached || args.Contains("--console"));
            Console.WriteLine($"{nameof(_isService)}: {_isService}");

            var directoryConfig = GetDirectoryConfig();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddSingleton(s => directoryConfig);
                    services.AddHostedService<OrleansSiloHost>();
                });

            if (_isService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }

        private static DirectoryConfig GetDirectoryConfig()
        {
            if (_isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                return new DirectoryConfig()
                {
                    RootDirectory = Path.GetDirectoryName(pathToExe) + @"\"
                };
            }

            return new DirectoryConfig()
            {
                RootDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
        }
    }
}
