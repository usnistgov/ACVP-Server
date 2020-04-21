using System;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.TaskQueue;
using Serilog;

namespace MessageQueueProcessor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var directoryConfig = EntryPointConfigHelper.GetRootDirectory();
			CreateHostBuilder(args, directoryConfig).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args, string directoryConfig) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, builder) =>
				{
					var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
					if (string.IsNullOrWhiteSpace(env))
					{
						/* TODO this could fall back to an environment,
						 * when/if driver is updated to check for var
						 */
						throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
					}

					context.HostingEnvironment.EnvironmentName = env;
                    
					builder
						.AddJsonFile($"{directoryConfig}sharedappsettings.json", optional: false, reloadOnChange: false)
						.AddJsonFile($"{directoryConfig}sharedappsettings.{env}.json", optional: false, reloadOnChange: false)
						.AddJsonFile($"{directoryConfig}appsettings.json", optional: false, reloadOnChange: false)
						.AddJsonFile($"{directoryConfig}appsettings.{env}.json", optional: false, reloadOnChange: false);
				})
				.UseWindowsService()
				.UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration))

				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<Worker>();

					//Do the configuration thing
					services.Configure<MessageQueueProcessorConfig>(hostContext.Configuration.GetSection("MessageQueueProcessor"));

					//Inject libraries
					services.InjectACVPCore();
					services.InjectACVPWorkflow();
					services.InjectDatabaseInterface();
					services.InjectMessageQueue();
					services.InjectTaskQueue();

					//Inject local things
					services.AddSingleton<IMessageProcessorFactory, MessageProcessorFactory>();
				});
	}
}
