using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.TaskQueue.Providers;
using NIST.CVP.TaskQueue.Services;

namespace NIST.CVP.TaskQueue
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectTaskQueue(this IServiceCollection services)
		{
			services.AddSingleton<ITaskQueueProvider, TaskQueueProvider>();
			services.AddSingleton<ITaskQueueService, TaskQueueService>();
			return services;
		}
	}
}
