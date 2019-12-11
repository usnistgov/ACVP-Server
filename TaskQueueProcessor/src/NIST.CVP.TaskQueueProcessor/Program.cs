using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Services;
using NIST.CVP.Generation;
using NIST.CVP.Generation.Core;
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
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddEnvironmentVariables("ASPNETCORE_");
                    configHost.AddJsonFile("appsettings.json");
                })
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDbConnectionStringFactory, DbConnectionStringFactory>();
                    services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();

                    services.Configure<TaskQueueProcessorConfig>(hostContext.Configuration.GetSection(nameof(TaskQueueProcessorConfig)));
                    services.Configure<EnvironmentConfig>(hostContext.Configuration.GetSection(nameof(EnvironmentConfig)));
                    services.Configure<PoolConfig>(hostContext.Configuration.GetSection(nameof(PoolConfig)));
                    
                    services.AddSingleton<IGenValInvoker, GenValInvoker>();
                    services.AddSingleton<ITaskRetriever, TaskRetriever>();
                    services.AddSingleton<IDbProvider, DbProvider>();
                    
                    services.AddSingleton<ITaskRunner, TaskRunner>();
                    services.AddTransient<IPoolProvider, PoolProvider>();
                    
                    services.AddHostedService<QueueProcessor>();
                });
    }
}