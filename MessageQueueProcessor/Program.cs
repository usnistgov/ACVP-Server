using ACVPCore;
using CVP.DatabaseInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessageQueueProcessor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseWindowsService()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<Worker>();

					//Inject libraries
					services.InjectACVPCore();
					services.InjectDatabaseInterface();

					//Inject local things
					services.AddSingleton<IMessageProvider, MessageProvider>();
					services.AddSingleton<IMessageProcessorFactory, MessageProcessorFactory>();
				});
	}
}
