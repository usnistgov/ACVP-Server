using ACVPCore;
using ACVPWorkflow;
using CVP.DatabaseInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
				.UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration))

				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<Worker>();

					//Inject libraries
					services.InjectACVPCore();
					services.InjectACVPWorkflow();
					services.InjectDatabaseInterface();

					//Inject local things
					services.AddSingleton<IMessageProvider, MessageProvider>();
					services.AddSingleton<IMessageProcessorFactory, MessageProcessorFactory>();
					services.AddSingleton<IAutoApproveProvider, AutoApproveProvider>();
				});
	}
}
