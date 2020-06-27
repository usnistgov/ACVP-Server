using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Generation;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Services;

namespace NIST.CVP.TaskQueueProcessor
{
	public static class IServiceCollectionExtension
    {
        public static IServiceCollection InjectTaskQueueProcessorInterfaces(this IServiceCollection services)
        {
            services.AddSingleton<IGenValInvoker, GenValInvoker>();
            services.AddSingleton<IGenValService, GenValService>();
            services.AddSingleton<IPoolService, PoolService>();
                    
            services.AddSingleton<ITaskService, TaskService>();
                    
            services.AddHostedService<QueueProcessor>();
            
            return services;
        }
    }
}