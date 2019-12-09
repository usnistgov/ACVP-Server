using ACVPCore.Providers;
using ACVPCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ACVPCore
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectACVPCore(this IServiceCollection services)
		{
			services.AddSingleton<ITestSessionService, TestSessionService>();
			services.AddSingleton<ITestSessionProvider, TestSessionProvider>();
			services.AddSingleton<IVectorSetService, VectorSetService>();
			services.AddSingleton<IVectorSetProvider, VectorSetProvider>();
			services.AddSingleton<ITaskQueueService, TaskQueueService>();
			services.AddSingleton<ITaskQueueProvider, TaskQueueProvider>();
			services.AddSingleton<IVectorSetExpectedResultsProvider, VectorSetExpectedResultsProvider>();
			services.AddSingleton<IDependencyService, DependencyService>();
			services.AddSingleton<IDependencyProvider, DependencyProvider>();
			return services;
		}
	}
}
