using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Libraries.Internal.TaskQueue.Providers;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;

namespace NIST.CVP.Libraries.Internal.TaskQueue
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
