using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                var index = IndexOf(args, "--poolUrl");
                PoolUrl = args[index + 1];
            }

            if (args.Contains("--output"))
            {
                var index = IndexOf(args, "--output");
                OutputFilePath = args[index + 1];
            }

            if (args.Contains("--interval"))
            {
                var index = IndexOf(args, "--interval");
                IntervalSeconds = int.Parse(args[index + 1]);
            }

            if (args.Contains("--error"))
            {
                var index = IndexOf(args, "--error");
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

        private static int IndexOf(string[] array, string value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
