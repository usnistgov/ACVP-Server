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
            services.AddSingleton<IGenValInvoker, GenValInvoker>();
            services.AddSingleton<IGenValService, GenValService>();
            services.AddSingleton<ITaskProvider, TaskProvider>();
            services.AddSingleton<IPoolService, PoolService>();
                    
            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<ICleaningService, CleaningService>();
                    
            services.AddHostedService<QueueProcessor>();
            
            return services;
        }
    }
}