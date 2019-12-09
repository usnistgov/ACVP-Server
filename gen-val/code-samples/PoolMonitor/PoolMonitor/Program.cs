using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PoolMonitor
{
    public class Program
    {
        // Default values
        public static string PoolUrl = "http://admin.dev.acvts.nist.gov:5002/api/pools/";
        public static string OutputFilePath = @"\\elwood\773\internal\STVMDev\acvp\PoolAPITestResults\poolStatusData.csv";
        public static int IntervalSeconds = 300;
        public static string ErrorFilePath = @"\\elwood\773\internal\STVMDev\acvp\PoolAPITestResults\httpError.log";

        public static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (args.Contains("--poolUrl"))
            {
                var index = args.IndexOf("--poolUrl");
                PoolUrl = args[index + 1];
            }

            if (args.Contains("--output"))
            {
                var index = args.IndexOf("--output");
                OutputFilePath = args[index + 1];
            }

            if (args.Contains("--interval"))
            {
                var index = args.IndexOf("--interval");
                IntervalSeconds = int.Parse(args[index + 1]);
            }

            if (args.Contains("--error"))
            {
                var index = args.IndexOf("--error");
                ErrorFilePath = args[index + 1];
            }

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<PoolQueryService>();
                });

            if (isService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }
    }
}
