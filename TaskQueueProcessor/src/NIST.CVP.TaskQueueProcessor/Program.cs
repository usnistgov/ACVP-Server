using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NIST.CVP.TaskQueueProcessor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<QueueProcessor>();
                });
    }
}