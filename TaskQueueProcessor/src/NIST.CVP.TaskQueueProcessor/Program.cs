using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common;
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
                    services.AddSingleton(new LimitedConcurrencyLevelTaskScheduler(1));

                    services.AddHttpClient();
                    
                    services.Configure<TaskQueueProcessorConfig>(hostContext.Configuration.GetSection(nameof(TaskQueueProcessorConfig)));
                    services.Configure<EnvironmentConfig>(hostContext.Configuration.GetSection(nameof(EnvironmentConfig)));
                    services.Configure<PoolConfig>(hostContext.Configuration.GetSection(nameof(PoolConfig)));
                    services.Configure<OrleansConfig>(hostContext.Configuration.GetSection(nameof(OrleansConfig)));
                    
                    services.AddSingleton<IGenValInvoker, GenValInvoker>();
                    services.AddSingleton<ITaskRetriever, TaskRetriever>();
                    services.AddSingleton<IDbProvider, DbProvider>();
                    
                    services.AddSingleton<ITaskRunner, TaskRunner>();
                    services.AddTransient<IPoolProvider, PoolProvider>();
                    
                    services.AddHostedService<QueueProcessor>();
                });
    }
}