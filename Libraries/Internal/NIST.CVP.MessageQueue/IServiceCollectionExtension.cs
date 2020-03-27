using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.MessageQueue.Providers;
using NIST.CVP.MessageQueue.Services;

namespace NIST.CVP.MessageQueue
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
