using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common;
using NIST.CVP.Generation;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.Services;

namespace NIST.CVP.TaskQueueProcessor
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection InjectTaskQueueProcessorInterfaces(this IServiceCollection services)
        {
            services.AddSingleton(new LimitedConcurrencyLevelTaskScheduler(1));
            services.AddTransient<IGenValInvoker, GenValInvoker>();
            services.AddTransient<IGenValService, GenValService>();
            services.AddTransient<ITaskProvider, TaskProvider>();
            services.AddTransient<IPoolService, PoolService>();
            services.AddTransient<IJsonProvider, JsonProvider>();
                    
            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<ICleaningService, CleaningService>();
                    
            services.AddHostedService<QueueProcessor>();
            
            return services;
        }
    }
}