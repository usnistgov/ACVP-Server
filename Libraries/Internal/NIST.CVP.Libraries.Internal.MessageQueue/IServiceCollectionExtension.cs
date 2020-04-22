using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Libraries.Internal.MessageQueue.Providers;
using NIST.CVP.Libraries.Internal.MessageQueue.Services;

namespace NIST.CVP.Libraries.Internal.MessageQueue
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectMessageQueue(this IServiceCollection services)
		{
			services.AddSingleton<IMessageQueueProvider, MessageQueueProvider>();
			services.AddSingleton<IMessageQueueService, MessageQueueService>();
			return services;
		}
	}
}
