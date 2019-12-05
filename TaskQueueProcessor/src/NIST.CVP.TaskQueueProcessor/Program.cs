using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Generation;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor
{
    public static class Program
    {
        private const string CONNECTION_STRING = "Server=localhost;Database=Acvp;User=SA;Password=Password123;";
        private const string POOL_URL = "localhost";
        private const int POOL_PORT = 5002;
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    var genValInvoker = new GenValInvoker(services.BuildServiceProvider());
                    var taskRetriever = new TaskRetriever(genValInvoker);
                    IDbProvider dbProvider = new DbProvider(taskRetriever, CONNECTION_STRING);
                    services.AddSingleton(dbProvider);
                    
                    services.AddSingleton<ITaskRunner, TaskRunner>();
                    services.AddTransient(_ =>
                    {
                        IPoolProvider poolProvider = new PoolProvider(POOL_URL, POOL_PORT);
                        return poolProvider;
                    });
                    
                    services.AddHostedService<QueueProcessor>();
                });
    }
}